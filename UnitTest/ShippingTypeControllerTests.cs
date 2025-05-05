using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ITI.Shipping.APIs.Controllers;
using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Application.Abstraction.ShippingType.Model;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITI.Shipping.Core.Application.Abstraction.Region.Model;

public class ShippingTypeControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly ShippingTypeController _controller;

    public ShippingTypeControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new ShippingTypeController(_serviceManagerMock.Object);
    }

    [Fact]
    public async Task GetAllShippingType_ReturnsOkResult_WithListOfShippingTypes()
    {
        // Arrange
        var mockShippingTypes = new List<ShippingTypeDTO>
        {
            new ShippingTypeDTO { Id = 1, Name = "Type1" },
            new ShippingTypeDTO { Id = 2, Name = "Type2" }
        };
        _serviceManagerMock
            .Setup(s => s.shippingTypeService.GetAllShippingTypeAsync(It.IsAny<Pramter>()))
            .ReturnsAsync(mockShippingTypes);

        // Act
        var result = await _controller.GetAllShippingType(new Pramter());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedShippingTypes = Assert.IsType<List<ShippingTypeDTO>>(okResult.Value);
        Assert.Equal(2, returnedShippingTypes.Count);
    }

    [Fact]
    public async Task GetShippingType_ReturnsOkResult_WithShippingType()
    {
        // Arrange
        var mockShippingType = new ShippingTypeDTO { Id = 1, Name = "Type1" };
        _serviceManagerMock
            .Setup(s => s.shippingTypeService.GetShippingTypeAsync(1))
            .ReturnsAsync(mockShippingType);

        // Act
        var result = await _controller.GetShippingType(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedShippingType = Assert.IsType<ShippingTypeDTO>(okResult.Value);
        Assert.Equal(1, returnedShippingType.Id);
    }

    [Fact]
    public async Task AddShippingType_ReturnsOkResult_WhenShippingTypeIsAdded()
    {
        // Arrange
        var shippingTypeToAdd = new ShippingTypeDTO { Id = 1, Name = "NewType" };
        _serviceManagerMock
            .Setup(s => s.shippingTypeService.AddAsync(shippingTypeToAdd))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddShippingType(shippingTypeToAdd);
        var actionResult = Assert.IsType<ActionResult<ShippingTypeDTO>>(result);
        // Assert
        Assert.IsType<OkResult>(actionResult.Result);
    }

    [Fact]
    public async Task AddShippingType_ReturnsBadRequest_WhenShippingTypeIsNull()
    {
        // Act
        var result = await _controller.AddShippingType(null);

        // Assert
        var actionResult = Assert.IsType<ActionResult<ShippingTypeDTO>>(result);
        var badrequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        Assert.Equal("Invalid ShippingType data",badrequestResult.Value);
    }

    [Fact]
    public async Task DeleteShippingType_ReturnsNoContent_WhenShippingTypeIsDeleted()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.shippingTypeService.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteShippingType(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteShippingType_ReturnsNotFound_WhenShippingTypeDoesNotExist()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.shippingTypeService.DeleteAsync(1))
            .Throws(new KeyNotFoundException("ShippingType not found"));

        // Act
        var result = await _controller.DeleteShippingType(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("ShippingType not found", notFoundResult.Value);
    }
}
