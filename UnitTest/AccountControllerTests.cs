using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ITI.Shipping.APIs.Controllers;
using ITI.Shipping.Core.Application.Abstraction.User;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ITI.Shipping.Core.Application.Abstraction.Auth.Model;

public class AccountControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new AccountController(_userServiceMock.Object);

        // Mock the User context for the controller
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "12345") // Mocked user ID
        }, "mock"));
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task GetAccountProfile_ReturnsOkResult_WithAccountProfile()
    {
        // Arrange
        var mockProfile = new AccountProfileDTO
        (
            Email :"john.doe@example.com",
            FullName: "John Doe",
            PhoneNumber: "1234567890",
            Address: "123 Main St"
        );
        _userServiceMock
            .Setup(s => s.GetAccountProfileAsync("12345",default))
            .ReturnsAsync(mockProfile);

        // Act
        var result = await _controller.GetAccountProfile();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProfile = Assert.IsType<AccountProfileDTO>(okResult.Value);
        Assert.Equal("John Doe",returnedProfile.FullName);
        Assert.Equal("john.doe@example.com",returnedProfile.Email);
    }

    [Fact]
    public async Task GetAccountProfile_ReturnsNotFound_WhenProfileIsNull()
    {
        // Arrange
        _userServiceMock
            .Setup(s => s.GetAccountProfileAsync("12345",default))
            .ReturnsAsync((AccountProfileDTO) null);

        // Act
        var result =  _controller.GetAccountProfile().Result;

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAccountProfile_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() } // No claims
        };

        // Act
        var result = await _controller.GetAccountProfile();

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }
}
