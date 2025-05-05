using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ITI.Shipping.APIs.Controllers;
using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Application.Abstraction.CourierReport.Model;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CourierReportControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly CourierReportController _controller;

    public CourierReportControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new CourierReportController(_serviceManagerMock.Object);
    }

    [Fact]
    public async Task GetAllReports_ReturnsOkResult_WithListOfReports()
    {
        // Arrange
        var mockReports = new List<GetAllCourierOrderCountDto>
        {
            new GetAllCourierOrderCountDto { CourierName = "Courier1", OrdersCount = 5 },
            new GetAllCourierOrderCountDto { CourierName = "Courier2", OrdersCount = 10 }
        };
        _serviceManagerMock
            .Setup(s => s.CourierReportService.GetAllCourierReportsAsync(It.IsAny<Pramter>()))
            .ReturnsAsync(mockReports);

        // Act
        var result = await _controller.GetAllReports(new Pramter());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedReports = Assert.IsType<List<GetAllCourierOrderCountDto>>(okResult.Value);
        Assert.Equal(2, returnedReports.Count);
    }

    [Fact]
    public async Task GetAllReports_ReturnsEmptyList_WhenNoReportsExist()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.CourierReportService.GetAllCourierReportsAsync(It.IsAny<Pramter>()))
            .ReturnsAsync(new List<GetAllCourierOrderCountDto>());

        // Act
        var result = await _controller.GetAllReports(new Pramter());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedReports = Assert.IsType<List<GetAllCourierOrderCountDto>>(okResult.Value);
        Assert.Empty(returnedReports);
    }
}
