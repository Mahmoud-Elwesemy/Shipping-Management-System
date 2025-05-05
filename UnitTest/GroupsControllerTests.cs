using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ITI.Shipping.APIs.Controllers;
using ITI.Shipping.Core.Application.Abstraction.Auth;
using ITI.Shipping.Core.Application.Abstraction.Auth.Model;
using ITI.Shipping.Core.Domin.ResponseHelper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class GroupsControllerTests
{
    private readonly Mock<IRoleService> _roleServiceMock;
    private readonly GroupsController _controller;

    public GroupsControllerTests()
    {
        _roleServiceMock = new Mock<IRoleService>();
        _controller = new GroupsController(_roleServiceMock.Object);
    }

    [Fact]
    public async Task GetAllGroups_ReturnsOkResult_WithListOfGroups()
    {
        // Arrange
        var mockGroups = new List<RoleResponseDTO>
        {
            new RoleResponseDTO ( RoleId : "1", RoleName : "Group1", CreatedAt : "2023-01-01" ),
            new RoleResponseDTO (RoleId : "2", RoleName : "Group2", CreatedAt : "2023-01-02")
        };
        _roleServiceMock
            .Setup(s => s.GetAllRolesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockGroups);

        // Act
        var result = await _controller.GetAllGroups(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGroups = Assert.IsType<List<RoleResponseDTO>>(okResult.Value);
        Assert.Equal(2,returnedGroups.Count);
    }

    [Fact]
    public async Task GetGroup_ReturnsOkResult_WithGroup()
    {
        // Arrange
        var mockGroup = new RoleDetailsResponseDTO(
            RoleId: "1",
            RoleName: "Group1",
            CreatedAt: "2023-01-01",
            Permissions: new List<string>()
        );
        _roleServiceMock
            .Setup(s => s.GetRoleByIdAsync("1",It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockGroup);

        // Act
        var result = await _controller.GetGroup("1",CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGroup = Assert.IsType<RoleDetailsResponseDTO>(okResult.Value);
        Assert.Equal("1",returnedGroup.RoleId);
    }

    [Fact]
    public async Task GetGroup_ReturnsNotFound_WhenGroupDoesNotExist()
    {
        // Arrange
        _roleServiceMock
            .Setup(s => s.GetRoleByIdAsync("1", It.IsAny<CancellationToken>()))
            .ReturnsAsync((RoleDetailsResponseDTO)null);

        // Act
        var result = await _controller.GetGroup("1", CancellationToken.None);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var response = Assert.IsType<ResponseAPI>(notFoundResult.Value);
        Assert.Equal("Group does not exists", response.Message);
    }

    [Fact]
    public async Task CreateGroup_ReturnsOkResult_WhenGroupIsCreated()
    {
        // Arrange
        var createRequest = new CreateRoleRequestDTO
        (
            RoleName : "NewGroup",
            Permissions : new List<string>()
        );
        _roleServiceMock
            .Setup(s => s.CreateRoleAsync(createRequest,It.IsAny<CancellationToken>()))
            .ReturnsAsync("Group Created Successfully");

        // Act
        var result = await _controller.CreateGroup(createRequest,CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseAPI>(okResult.Value);
        Assert.Equal("Group Created Successfully",response.Message);
    }

    [Fact]
    public async Task DeleteGroup_ReturnsOkResult_WhenGroupIsDeleted()
    {
        // Arrange
        _roleServiceMock
            .Setup(s => s.DeleteRoleAsync("1", It.IsAny<CancellationToken>()))
            .ReturnsAsync("Group Deleted Successfully");

        // Act
        var result = await _controller.DeleteGroup("1", CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseAPI>(okResult.Value);
        Assert.Equal("Group Deleted Successfully", response.Message);
    }
}
