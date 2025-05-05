using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ITI.Shipping.APIs.Controllers;
using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Application.Abstraction.Branch.Models;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

public class BranchControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly BranchController _controller;

    public BranchControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new BranchController(_serviceManagerMock.Object);
    }

    [Fact]
    public async Task GetBranches_ReturnsOkResult_WithListOfBranches()
    {
        // Arrange
        var mockBranches = new List<BranchDTO>
        {
            new BranchDTO { Id = 1, Name = "Branch1", Location = "Location1" },
            new BranchDTO { Id = 2, Name = "Branch2", Location = "Location2" }
        };
        _serviceManagerMock
            .Setup(s => s.BranchService.GetBranchesAsync(It.IsAny<Pramter>()))
            .ReturnsAsync(mockBranches);

        // Act
        var result = await _controller.GetBranches(new Pramter());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedBranches = Assert.IsType<List<BranchDTO>>(okResult.Value);
        Assert.Equal(2, returnedBranches.Count);
    }

    [Fact]
    public async Task GetBranches_ReturnsOkResult_WithEmptyList()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.BranchService.GetBranchesAsync(It.IsAny<Pramter>()))
            .ReturnsAsync(new List<BranchDTO>());

        // Act
        var result = await _controller.GetBranches(new Pramter());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedBranches = Assert.IsType<List<BranchDTO>>(okResult.Value);
        Assert.Empty(returnedBranches);
    }

    [Fact]
    public async Task GetBranch_ReturnsOkResult_WithBranch()
    {
        // Arrange
        var mockBranch = new BranchDTO { Id = 1, Name = "Branch1", Location = "Location1" };
        _serviceManagerMock
            .Setup(s => s.BranchService.GetBranchAsync(1))
            .ReturnsAsync(mockBranch);

        // Act
        var result = await _controller.GetBranch(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedBranch = Assert.IsType<BranchDTO>(okResult.Value);
        Assert.Equal(1, returnedBranch.Id);
        Assert.Equal("Branch1", returnedBranch.Name);
    }

    [Fact]
    public async Task GetBranch_ReturnsNotFound_WhenBranchDoesNotExist()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.BranchService.GetBranchAsync(1))
            .ReturnsAsync((BranchDTO)null);

        // Act
        var result = await _controller.GetBranch(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task UpdateBranch_ReturnsNoContent_WhenBranchIsUpdated()
    {
        // Arrange
        var branchToUpdate = new BranchToUpdateDTO { Id = 1, Name = "UpdatedBranch", Location = "UpdatedLocation" };
        _serviceManagerMock
            .Setup(s => s.BranchService.UpdateAsync(branchToUpdate))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateBranch(1, branchToUpdate);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateBranch_ReturnsBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var branchToUpdate = new BranchToUpdateDTO { Id = 2, Name = "UpdatedBranch", Location = "UpdatedLocation" };

        // Act
        var result = await _controller.UpdateBranch(1, branchToUpdate);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid branch data.", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdateBranch_ReturnsNotFound_WhenBranchDoesNotExist()
    {
        // Arrange
        var branchToUpdate = new BranchToUpdateDTO { Id = 1, Name = "UpdatedBranch", Location = "UpdatedLocation" };
        _serviceManagerMock
            .Setup(s => s.BranchService.UpdateAsync(branchToUpdate))
            .Throws(new KeyNotFoundException("Branch not found"));

        // Act
        var result = await _controller.UpdateBranch(1, branchToUpdate);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Branch not found", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteBranch_ReturnsNoContent_WhenBranchIsDeleted()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.BranchService.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteBranch(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteBranch_ReturnsNotFound_WhenBranchDoesNotExist()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.BranchService.DeleteAsync(1))
            .Throws(new KeyNotFoundException("Branch not found"));

        // Act
        var result = await _controller.DeleteBranch(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Branch not found", notFoundResult.Value);
    }
}
