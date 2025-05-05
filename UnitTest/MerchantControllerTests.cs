using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ITI.Shipping.APIs.Controllers;
using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Application.Abstraction.Merchant.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MerchantControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly MerchantController _controller;

    public MerchantControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new MerchantController(_serviceManagerMock.Object);
    }

    [Fact]
    public async Task GetAllMerchant_ReturnsOkResult_WithListOfMerchants()
    {
        // Arrange
        var mockMerchants = new List<MerchantDTO>
        {
            new MerchantDTO { Id = "1", Name = "Merchant1" },
            new MerchantDTO { Id = "2", Name = "Merchant2" }
        };
        _serviceManagerMock
            .Setup(s => s.merchantService.GetMerchantAsync())
            .ReturnsAsync(mockMerchants);

        // Act
        var result = await _controller.GetAllMerchant();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedMerchants = Assert.IsType<List<MerchantDTO>>(okResult.Value);
        Assert.Equal(2, returnedMerchants.Count);
    }

    [Fact]
    public async Task GetAllMerchant_ReturnsOkResult_WithEmptyList()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.merchantService.GetMerchantAsync())
            .ReturnsAsync(new List<MerchantDTO>());

        // Act
        var result = await _controller.GetAllMerchant();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedMerchants = Assert.IsType<List<MerchantDTO>>(okResult.Value);
        Assert.Empty(returnedMerchants);
    }
}
