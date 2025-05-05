using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ITI.Shipping.APIs.Controllers;
using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Application.Abstraction.Courier.DTO;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CourierControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly CourierController _controller;

    public CourierControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new CourierController(_serviceManagerMock.Object);
    }

    [Fact]
    public async Task GetCouriersByBranch_ReturnsOkResult_WithListOfCouriers()
    {
        // Arrange
        var mockCouriers = new List<CourierDTO>
        {
            new CourierDTO { CourierId = "1", CourierName = "Courier1" },
            new CourierDTO { CourierId = "2", CourierName = "Courier2" }
        };
        _serviceManagerMock
            .Setup(s => s.courierService.GetCourierByBranch(1))
            .ReturnsAsync(mockCouriers);

        // Act
        var result = await _controller.GetCouriersByBranch(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCouriers = Assert.IsType<List<CourierDTO>>(okResult.Value);
        Assert.Equal(2, returnedCouriers.Count);
    }

    [Fact]
    public async Task GetCouriersByRegion_ReturnsOkResult_WithListOfCouriers()
    {
        // Arrange
        var mockCouriers = new List<CourierDTO>
        {
            new CourierDTO { CourierId = "1", CourierName = "Courier1" }
        };
        _serviceManagerMock
            .Setup(s => s.courierService.GetCourierByRegion(1, It.IsAny<Pramter>()))
            .ReturnsAsync(mockCouriers);

        // Act
        var result = await _controller.GetCouriersByRegion(1, new Pramter());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCouriers = Assert.IsType<List<CourierDTO>>(okResult.Value);
        Assert.Single(returnedCouriers);
    }
}
