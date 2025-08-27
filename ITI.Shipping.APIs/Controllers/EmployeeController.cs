using ITI.Shipping.APIs.Filters;
using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Application.Abstraction.Employee.Model;
using ITI.Shipping.Core.Domin.Entities_Helper;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Shipping.APIs.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmployeeController:ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public EmployeeController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }
    [HttpGet("GetAllEmployees")]
    [HasPermission(Permissions.ViewEmployees)]
    public async Task<ActionResult> GetAllEmployees([FromQuery] Pramter pramter)
    {
        var employees = await _serviceManager.employeeService.GetAllEmployeesAsync(pramter);
        return Ok(employees);
    }
    [HttpGet("GetEmployeeById/{id}")]
    [HasPermission(Permissions.ViewEmployees)]
    public async Task<ActionResult> GetEmployeeById(string id)
    {
        var employee = await _serviceManager.employeeService.GetEmployeeByIdAsync(id);
        return Ok(employee);
    }
    [HttpPut("UpdateEmployee")]
    [HasPermission(Permissions.UpdateEmployees)]
    public async Task<ActionResult> UpdateEmployee([FromBody] EmployeeUpdateDTO employeeDTO)
    {
        if (employeeDTO == null || string.IsNullOrEmpty(employeeDTO.Id))
        {
            return BadRequest("Invalid employee data.");
        }
        
        await _serviceManager.employeeService.UpdateAsync(employeeDTO);
        return NoContent();
    }
    [HttpDelete("DeleteEmployee/{id}")]
    [HasPermission(Permissions.DeleteEmployees)]
    public async Task<ActionResult> DeleteEmployee(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Invalid employee ID.");
        }
        
        await _serviceManager.employeeService.DeleteAsync(id);
        return NoContent();
    }
}
