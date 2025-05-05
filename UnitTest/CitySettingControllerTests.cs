using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ITI.Shipping.APIs.Controllers;
using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Application.Abstraction.CitySetting.Models;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CitySettingControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly CitySettingController _controller;

    public CitySettingControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new CitySettingController(_serviceManagerMock.Object);
    }

    [Fact]
    public async Task GetCitySettings_ReturnsOkResult_WithListOfCitySettings()
    {
        // Arrange
        var mockCitySettings = new List<CitySettingDTO>
        {
            new CitySettingDTO { Id = 1, Name = "City1", StandardShippingCost = 10 },
            new CitySettingDTO { Id = 2, Name = "City2", StandardShippingCost = 20 }
        };
        _serviceManagerMock
            .Setup(s => s.CitySettingService.GetCitySettingsAsync(It.IsAny<Pramter>()))
            .ReturnsAsync(mockCitySettings);

        // Act
        var result = await _controller.GetCitySettings(new Pramter());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCitySettings = Assert.IsType<List<CitySettingDTO>>(okResult.Value);
        Assert.Equal(2, returnedCitySettings.Count);
    }

    [Fact]
    public async Task GetCityByGovernorateName_ReturnsOkResult_WithListOfCitySettings()
    {
        // Arrange
        var mockCitySettings = new List<CitySettingDTO>
        {
            new CitySettingDTO { Id = 1, Name = "City1", StandardShippingCost = 10 }
        };
        _serviceManagerMock
            .Setup(s => s.CitySettingService.GetCityByGovernorateName(1))
            .ReturnsAsync(mockCitySettings);

        // Act
        var result = await _controller.GetCityByGovernorateName(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCitySettings = Assert.IsType<List<CitySettingDTO>>(okResult.Value);
        Assert.Single(returnedCitySettings);
    }

    [Fact]
    public async Task GetCitySetting_ReturnsOkResult_WithCitySetting()
    {
        // Arrange
        var mockCitySetting = new CitySettingDTO { Id = 1, Name = "City1", StandardShippingCost = 10 };
        _serviceManagerMock
            .Setup(s => s.CitySettingService.GetCitySettingAsync(1))
            .ReturnsAsync(mockCitySetting);

        // Act
        var result = await _controller.GetCitySetting(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCitySetting = Assert.IsType<CitySettingDTO>(okResult.Value);
        Assert.Equal(1, returnedCitySetting.Id);
    }


    [Fact]
    public async Task AddCitySetting_ReturnsActionResult_WithCitySettingToAddDTO()
    {
        // Arrange
        var cityToAdd = new CitySettingToAddDTO { Name = "NewCity",StandardShippingCost = 15 };
        _serviceManagerMock
            .Setup(s => s.CitySettingService.AddAsync(cityToAdd))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddCitySetting(cityToAdd);

        // Assert
        var actionResult = Assert.IsType<ActionResult<CitySettingToAddDTO>>(result);
        Assert.IsType<OkResult>(actionResult.Result);
    }

    [Fact]
    public async Task UpdateCitySetting_ReturnsNoContent_WhenCitySettingIsUpdated()
    {
        // Arrange
        var cityToUpdate = new CitySettingToUpdateDTO { Id = 1, Name = "UpdatedCity", StandardShippingCost = 20 };
        _serviceManagerMock
            .Setup(s => s.CitySettingService.UpdateAsync(cityToUpdate))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateCitySetting(1, cityToUpdate);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCitySetting_ReturnsNoContent_WhenCitySettingIsDeleted()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.CitySettingService.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteCitySetting(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
