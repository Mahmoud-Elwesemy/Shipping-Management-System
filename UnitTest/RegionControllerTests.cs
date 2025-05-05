using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ITI.Shipping.APIs.Controllers;
using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Application.Abstraction.Region.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITI.Shipping.Core.Domin.Pramter_Helper;

public class RegionControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly RegionController _controller;

    public RegionControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new RegionController(_serviceManagerMock.Object);
    }

    [Fact]
    public async Task GetAllRegion_ReturnsOkResult_WithListOfRegions()
    {
        // Arrange
        var mockRegions = new List<RegionDto>
        {
            new RegionDto { Id = 1, Governorate = "Region1" },
            new RegionDto { Id = 2, Governorate = "Region2" }
        };
        _serviceManagerMock
            .Setup(s => s.RegionService.GetRegionsAsync(It.IsAny<Pramter>()))
            .ReturnsAsync(mockRegions);

        // Act
        var result = await _controller.GetAllRegion(new Pramter());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedRegions = Assert.IsType<List<RegionDto>>(okResult.Value);
        Assert.Equal(2,returnedRegions.Count);
    }

    [Fact]
    public async Task GetRegion_ReturnsOkResult_WithRegion()
    {
        // Arrange
        var mockRegion = new RegionDto { Id = 1,Governorate = "Region1" };
        _serviceManagerMock
            .Setup(s => s.RegionService.GetRegionAsync(1))
            .ReturnsAsync(mockRegion);

        // Act
        var result = await _controller.GetRegion(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedRegion = Assert.IsType<RegionDto>(okResult.Value);
        Assert.Equal(1,returnedRegion.Id);
    }

    [Fact]
    public async Task GetRegion_ReturnsNotFound_WhenRegionDoesNotExist()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.RegionService.GetRegionAsync(1))
            .ReturnsAsync((RegionDto) null);

        // Act
        var result = await _controller.GetRegion(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task AddRegion_ReturnsActionResult_WithOkResult()
    {
        // Arrange
        var regionToAdd = new RegionDto { Id = 1,Governorate = "NewRegion" };
        _serviceManagerMock
            .Setup(s => s.RegionService.AddAsync(regionToAdd))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddRegion(regionToAdd);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RegionDto>>(result);
        Assert.IsType<OkResult>(actionResult.Result);
    }

    [Fact]
    public async Task AddRegion_ReturnsActionResult_WithBadRequestResult_WhenRegionIsNull()
    {
        // Act
        var result = await _controller.AddRegion(null);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RegionDto>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task UpdateRegion_ReturnsActionResult_WithNoContentResult_WhenRegionIsUpdated()
    {
        // Arrange
        var regionToUpdate = new RegionDto { Id = 1,Governorate = "UpdatedRegion" };
        _serviceManagerMock
            .Setup(s => s.RegionService.UpdateAsync(regionToUpdate))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateRegion(1,regionToUpdate);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RegionDto>>(result);
        Assert.IsType<NoContentResult>(actionResult.Result);
    }


    [Fact]
    public async Task UpdateRegion_ReturnsActionResult_WithBadRequestResult_WhenIdDoesNotMatch()
    {
        // Arrange
        var regionToUpdate = new RegionDto { Id = 2,Governorate = "UpdatedRegion" };

        // Act
        var result = await _controller.UpdateRegion(1,regionToUpdate);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RegionDto>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }



    [Fact]
    public async Task UpdateRegion_ReturnsActionResult_WithNotFoundObjectResult_WhenRegionDoesNotExist()
    {
        // Arrange
        var regionToUpdate = new RegionDto { Id = 1,Governorate = "UpdatedRegion" };
        _serviceManagerMock
            .Setup(s => s.RegionService.UpdateAsync(regionToUpdate))
            .Throws(new KeyNotFoundException("Region not found"));

        // Act
        var result = await _controller.UpdateRegion(1,regionToUpdate);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RegionDto>>(result);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        Assert.Equal("Region not found",notFoundResult.Value);
    }


    [Fact]
    public async Task DeletRegion_ReturnsNoContent_WhenRegionIsDeleted()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.RegionService.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeletRegion(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeletRegion_ReturnsNotFound_WhenRegionDoesNotExist()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.RegionService.DeleteAsync(1))
            .Throws(new KeyNotFoundException("Region not found"));

        // Act
        var result = await _controller.DeletRegion(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Region not found",notFoundResult.Value);
    }
}
