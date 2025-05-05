using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ITI.Shipping.APIs.Controllers;
using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Application.Abstraction.Dashboard.DTO;
using System.Threading.Tasks;

public class DashboardControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly DashboardController _controller;

    public DashboardControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new DashboardController(_serviceManagerMock.Object);
    }

    [Fact]
    public async Task GetDashboardData_ReturnsOkResult_WithEmployeeDashboardData()
    {
        // Arrange
        var mockDashboardData = new EmpDashboardDTO { TotalDelivered = 10, TotalPending = 5 };
        _serviceManagerMock
            .Setup(s => s.dashboardService.GetDashboardOfEmployeeAsync())
            .ReturnsAsync(mockDashboardData);

        // Act
        var result = await _controller.GetDashboardData();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedData = Assert.IsType<EmpDashboardDTO>(okResult.Value);
        Assert.Equal(10, returnedData.TotalDelivered);
        Assert.Equal(5, returnedData.TotalPending);
    }

    [Fact]
    public async Task GetMerchantDashboardData_ReturnsOkResult_WithMerchantDashboardData()
    {
        // Arrange
        var mockDashboardData = new MerchantDashboardDTO { TotalDelivered = 15, TotalPending = 3 };
        _serviceManagerMock
            .Setup(s => s.dashboardService.GetDashboardDataForMerchantAsync())
            .ReturnsAsync(mockDashboardData);

        // Act
        var result = await _controller.GetMerchantDashboardData();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedData = Assert.IsType<MerchantDashboardDTO>(okResult.Value);
        Assert.Equal(15, returnedData.TotalDelivered);
        Assert.Equal(3, returnedData.TotalPending);
    }
}
