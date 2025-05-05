using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ITI.Shipping.APIs.Controllers;
using ITI.Shipping.Core.Application.Abstraction.User;
using ITI.Shipping.Core.Application.Abstraction.Auth.Model;
using ITI.Shipping.Core.Domin.ResponseHelper;
using System.Threading;
using System.Threading.Tasks;
using ITI.Shipping.Core.Application.Abstraction.User.Model;

public class AuthControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new AuthController(_userServiceMock.Object);
    }

    [Fact]
    public async Task Login_ReturnsOkResult_WithToken()
    {
        // Arrange
        var loginRequest = new LoginDTO(Email: "test@example.com",Password: "password");
        var mockResponse = new LoginResponseDTO(
            ID: "mock-id",
            Email: "test@example.com",
            FullName: "Test User",
            Token: "mock-token",
            ExpiresIn: 3600
        );
        _userServiceMock
            .Setup(s => s.GetTokenAsync(loginRequest,It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _controller.Login(loginRequest,CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedToken = Assert.IsType<LoginResponseDTO>(okResult.Value);
        Assert.Equal("mock-token",returnedToken.Token);
    }

    [Fact]
    public async Task Login_ReturnsBadRequest_WhenInvalidCredentials()
    {
        // Arrange
        var loginRequest = new LoginDTO(Email: "test@example.com",Password: "wrong-password");
        _userServiceMock
            .Setup(s => s.GetTokenAsync(loginRequest,It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginResponseDTO) null);

        // Act
        var result = await _controller.Login(loginRequest,CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<ResponseAPI>(badRequestResult.Value);
        Assert.Equal("Invalid Email or Password!",response.Message);
    }

    [Fact]
    public async Task AddEmployee_ReturnsCreatedResult()
    {
        // Fixing the instantiation of AddEmployeeDTO to include all required parameters
        var addEmployeeRequest = new AddEmployeeDTO
        (
            Email: "john.doe@example.com",
            Password: "password123",
            FullName: "John Doe",
            PhoneNumber: "1234567890",
            Address: "123 Main St",
            BranchId: 1,
            RegionID : 1,
            RoleName : "Employee"
        );
        _userServiceMock
            .Setup(s => s.AddEmployeeAsync(addEmployeeRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(string.Empty);

        // Act
        var result = await _controller.AddEmployee(addEmployeeRequest, CancellationToken.None);

        // Assert
        Assert.IsType<CreatedResult>(result);
    }

    [Fact]
    public async Task AddEmployee_ReturnsBadRequest_WhenInvalidRequest()
    {
        // Arrange
        var addEmployeeRequest = new AddEmployeeDTO
        (
            Email: "invalid-email",
            Password: "password123", 
            FullName: "John Doe",
            PhoneNumber: "1234567890",
            Address: "123 Main St",
            BranchId: 1,
            RegionID: 1,
            RoleName: "Employee"
        );
        _userServiceMock
            .Setup(s => s.AddEmployeeAsync(addEmployeeRequest,It.IsAny<CancellationToken>()))
            .ReturnsAsync("Invalid email format");

        // Act
        var result = await _controller.AddEmployee(addEmployeeRequest,CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<ResponseAPI>(badRequestResult.Value);
        Assert.Equal("Invalid email format",response.Message);
    }

    [Fact]
    public async Task AddMerchant_ReturnsCreatedResult()
    {
        // Arrange
        var addMerchantRequest = new AddMerchantDTO { FullName = "Merchant", Email = "merchant@example.com" };
        _userServiceMock
            .Setup(s => s.AddMerchantAsync(addMerchantRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(string.Empty);

        // Act
        var result = await _controller.AddMerchant(addMerchantRequest, CancellationToken.None);

        // Assert
        Assert.IsType<CreatedResult>(result);
    }

    [Fact]
    public async Task AddMerchant_ReturnsBadRequest_WhenInvalidRequest()
    {
        // Arrange
        var addMerchantRequest = new AddMerchantDTO { FullName = "Merchant", Email = "invalid-email" };
        _userServiceMock
            .Setup(s => s.AddMerchantAsync(addMerchantRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync("Invalid email format");

        // Act
        var result = await _controller.AddMerchant(addMerchantRequest, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<ResponseAPI>(badRequestResult.Value);
        Assert.Equal("Invalid email format", response.Message);
    }

    [Fact]
    public async Task AddCourier_ReturnsCreatedResult()
    {
        // Arrange
        var addCourierRequest = new AddCourierDTO { FullName = "Courier", Email = "courier@example.com" };
        _userServiceMock
            .Setup(s => s.AddCourierAsync(addCourierRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(string.Empty);

        // Act
        var result = await _controller.AddCourier(addCourierRequest, CancellationToken.None);

        // Assert
        Assert.IsType<CreatedResult>(result);
    }

    [Fact]
    public async Task AddCourier_ReturnsBadRequest_WhenInvalidRequest()
    {
        // Arrange
        var addCourierRequest = new AddCourierDTO { FullName = "Courier", Email = "invalid-email" };
        _userServiceMock
            .Setup(s => s.AddCourierAsync(addCourierRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync("Invalid email format");

        // Act
        var result = await _controller.AddCourier(addCourierRequest, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<ResponseAPI>(badRequestResult.Value);
        Assert.Equal("Invalid email format", response.Message);
    }
}
