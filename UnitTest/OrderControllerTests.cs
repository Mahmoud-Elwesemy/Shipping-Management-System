using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ITI.Shipping.APIs.Controllers;
using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Application.Abstraction.Order.Model;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using ITI.Shipping.Core.Domin.ResponseHelper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITI.Shipping.Core.Domin.Entities_Helper;

public class OrderControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly OrderController _controller;

    public OrderControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new OrderController(_serviceManagerMock.Object);
    }

    [Fact]
    public async Task GetAllOrdersByStatus_ReturnsOkResult_WithListOfOrders()
    {
        // Arrange
        var mockOrders = new List<OrderWithProductsDto>
        {
            new OrderWithProductsDto { Id = 1, Status = "Pending" },
            new OrderWithProductsDto { Id = 2, Status = "Delivered" }
        };
        _serviceManagerMock
            .Setup(s => s.orderService.GetOrdersByStatus(OrderStatus.Pending,It.IsAny<Pramter>()))
            .ReturnsAsync(mockOrders);

        // Act
        var result = await _controller.GetAllOrdersByStatus(OrderStatus.Pending,new Pramter());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedOrders = Assert.IsType<List<OrderWithProductsDto>>(okResult.Value);
        Assert.Equal(2,returnedOrders.Count);
    }

    [Fact]
    public async Task GetAllOrdersByStatus_ReturnsNotFound_WhenNoOrdersFound()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.orderService.GetOrdersByStatus(OrderStatus.Pending,It.IsAny<Pramter>()))
            .ReturnsAsync(new List<OrderWithProductsDto>());

        // Act
        var result = await _controller.GetAllOrdersByStatus(OrderStatus.Pending,new Pramter());

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var response = Assert.IsType<ResponseAPI>(notFoundResult.Value);
        Assert.Equal("No orders found",response.Message);
    }

    [Fact]
    public async Task AddOrder_ReturnsActionResult_WithBadRequestResult_WhenOrderIsNull()
    {
        // Act
        var result = await _controller.AddOrder(null);

        // Assert
        var actionResult = Assert.IsType<ActionResult<addOrderDto>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }


    [Fact]
    public async Task UpdateOrder_ReturnsNoContent_WhenOrderIsUpdated()
    {
        // Arrange
        var orderToUpdate = new updateOrderDto { Id = 1,OrderTypes = OrderType.Delivery,OrderCost = 150 };
        _serviceManagerMock
            .Setup(s => s.orderService.UpdateAsync(orderToUpdate))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateOrder(1,orderToUpdate);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateOrder_ReturnsBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var orderToUpdate = new updateOrderDto { Id = 2,OrderTypes = OrderType.Delivery,OrderCost = 150 };

        // Act
        var result = await _controller.UpdateOrder(1,orderToUpdate);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid branch data.",badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteOrder_ReturnsNoContent_WhenOrderIsDeleted()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.orderService.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteOrder(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteOrder_ReturnsNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.orderService.DeleteAsync(1))
            .Throws(new KeyNotFoundException("Order not found"));

        // Act
        var result = await _controller.DeleteOrder(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Order not found",notFoundResult.Value);
    }

    [Fact]
    public async Task AssignOrderToCourier_ReturnsNoContent_WhenOrderIsAssigned()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.orderService.AssignOrderToCourier(1,"courier123"))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AssignOrderToCourier(1,"courier123");

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task AssignOrderToCourier_ReturnsBadRequest_WhenCourierIdIsMissing()
    {
        // Act
        var result = await _controller.AssignOrderToCourier(1,"");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<ResponseAPI>(badRequestResult.Value);
        Assert.Equal("CourierId is required.",response.Message);
    }
    [Fact]
    public async Task AddOrder_ReturnsActionResult_WithOkResult()
    {
        // Arrange
        var orderToAdd = new addOrderDto { OrderTypes = OrderType.Delivery,OrderCost = 100 };
        _serviceManagerMock
            .Setup(s => s.orderService.AddAsync(orderToAdd))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddOrder(orderToAdd);

        // Assert
        var actionResult = Assert.IsType<ActionResult<addOrderDto>>(result);
        Assert.IsType<OkResult>(actionResult.Result);
    }
}

