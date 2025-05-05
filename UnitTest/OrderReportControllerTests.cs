using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ITI.Shipping.APIs.Controllers;
using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Application.Abstraction.OrderReport.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITI.Shipping.Core.Domin.Pramter_Helper;

public class OrderReportControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly OrderReportController _controller;

    public OrderReportControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new OrderReportController(_serviceManagerMock.Object);
    }

    [Fact]
    public async Task GetAllByPramter_ReturnsOkResult_WithListOfOrderReports()
    {
        // Arrange
        var mockReports = new List<OrderReportToShowDTO>
        {
            new OrderReportToShowDTO { Id = 1, MerchantName = "Merchant1" },
            new OrderReportToShowDTO { Id = 2, MerchantName = "Merchant2" }
        };
        _serviceManagerMock
            .Setup(s => s.orderReportService.GetAllOrderReportAsync(It.IsAny<OrderReportPramter>()))
            .ReturnsAsync(mockReports);

        // Act
        var result = await _controller.GetAllByPramter(new OrderReportPramter());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedReports = Assert.IsType<List<OrderReportToShowDTO>>(okResult.Value);
        Assert.Equal(2, returnedReports.Count);
    }
}
