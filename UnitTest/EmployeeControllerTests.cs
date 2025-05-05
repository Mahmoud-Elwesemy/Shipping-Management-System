using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ITI.Shipping.APIs.Controllers;
using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITI.Shipping.Core.Application.Abstraction.Employee.Model;

public class EmployeeControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly EmployeeController _controller;

    public EmployeeControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new EmployeeController(_serviceManagerMock.Object);
    }

    [Fact]
    public async Task GetAllEmployees_ReturnsOkResult_WithListOfEmployees()
    {
        // Arrange
        var mockEmployees = new List<EmployeeDTO>
                {
                    new EmployeeDTO { FullName = "Employee1", Email = "employee1@example.com" },
                    new EmployeeDTO { FullName = "Employee2", Email = "employee2@example.com" }
                };
        _serviceManagerMock
            .Setup(s => s.employeeService.GetEmployeesAsync(It.IsAny<Pramter>()))
            .ReturnsAsync(mockEmployees);

        // Act
        var result = await _controller.GetAllEmployees(new Pramter());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedEmployees = Assert.IsType<List<EmployeeDTO>>(okResult.Value);
        Assert.Equal(2,returnedEmployees.Count);
    }

    [Fact]
    public async Task GetAllEmployees_ReturnsOkResult_WithEmptyList()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.employeeService.GetEmployeesAsync(It.IsAny<Pramter>()))
            .ReturnsAsync(new List<EmployeeDTO>());

        // Act
        var result = await _controller.GetAllEmployees(new Pramter());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedEmployees = Assert.IsType<List<EmployeeDTO>>(okResult.Value);
        Assert.Empty(returnedEmployees);
    }
}
