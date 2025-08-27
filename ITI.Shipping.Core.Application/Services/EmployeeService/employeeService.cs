using AutoMapper;
using ITI.Shipping.Core.Application.Abstraction.Auth;
using ITI.Shipping.Core.Application.Abstraction.Employee;
using ITI.Shipping.Core.Application.Abstraction.Employee.Model;
using ITI.Shipping.Core.Domin.Entities;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using ITI.Shipping.Core.Domin.UnitOfWork.Contract;
using ITI.Shipping.Infrastructure.Presistence.UnitOfWork;
using Microsoft.AspNetCore.Identity;
namespace ITI.Shipping.Core.Application.Services.EmployeeService;
public class employeeService:IEmployeeService
{
    private readonly IUnitOfWork _UnitOfWork;
    private readonly IMapper _Mapper;
    private readonly IRoleService _roleService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public employeeService
        (IUnitOfWork unitOfWork,IMapper mapper ,IRoleService roleService 
        ,UserManager<ApplicationUser> userManager , RoleManager<ApplicationRole> roleManager)
    {
        _UnitOfWork = unitOfWork;
        _Mapper = mapper;
        _roleService = roleService;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    //------------------------------------------------------------------------

    private async Task UpdateEmployeeRoleAsync(ApplicationUser employee,string newRoleId)
    {
        
        var currentRoles = await _userManager.GetRolesAsync(employee);
        await _userManager.RemoveFromRolesAsync(employee,currentRoles);

        var newRole = await _roleManager.FindByIdAsync(newRoleId);
        if(newRole == null)
            throw new Exception($"Role with ID {newRoleId} not found");

        await _userManager.AddToRoleAsync(employee,newRole.Name);
    }

    //------------------------------------------------------------------------

    public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync(Pramter pramter)
    {
        var employees = _Mapper.Map<IEnumerable<EmployeeDTO>>(
            await _UnitOfWork.GetEmployeeRepository().GetAllEmployeesAsync(pramter)
        );

         foreach (var emp in employees)
        {
            try
            {
                var role = await _roleService.GetRolyByEmployeeIdAsync(emp.Id);
                emp.RoleId = role.RoleId;
                emp.RoleName = role.RoleName;
            }
            catch
            {
                emp.RoleId = string.Empty;
                emp.RoleName = "No Role";
            }
        }

        return employees;
    }
    //------------------------------------------------------------------------

    public async Task<EmployeeDTO> GetEmployeeByIdAsync(string id)
    {
       var employee = _Mapper.Map<EmployeeDTO>(await _UnitOfWork.GetEmployeeRepository()
           .GetByIdAsync(id));
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found.");
        }
        try
        {
            var role = await _roleService.GetRolyByEmployeeIdAsync(employee.Id);
            employee.RoleId = role.RoleId;
            employee.RoleName = role.RoleName;
        }
        catch
        {
            employee.RoleId = string.Empty;
            employee.RoleName = "No Role";
        }
        return employee;
    }
    //------------------------------------------------------------------------

    public async Task UpdateAsync(EmployeeUpdateDTO DTO)
    {
        var employeeRepo = _UnitOfWork.GetEmployeeRepository();
        var existingEmployee = await employeeRepo.GetByIdAsync(DTO.Id);

        if(existingEmployee == null)
            throw new KeyNotFoundException($"Employee with ID {DTO.Id} not found.");


        existingEmployee.FullName = DTO.FullName;
        existingEmployee.PhoneNumber = DTO.PhoneNumber;
        existingEmployee.BranchId = DTO.BranchId;
        existingEmployee.IsDeleted = DTO.IsDeleted;
        

        if(!string.IsNullOrEmpty(DTO.RoleId))
        {
            await UpdateEmployeeRoleAsync(existingEmployee,DTO.RoleId);
        }
        employeeRepo.UpdateAsync(existingEmployee);
        await _UnitOfWork.CompleteAsync();
    }
    //------------------------------------------------------------------------

    public async Task DeleteAsync(string id)
    {
        var employeeRepo = _UnitOfWork.GetEmployeeRepository();
        var existingEmployee =await employeeRepo.GetByIdAsync(id);
        if (existingEmployee == null)
            throw new KeyNotFoundException($"Employee with ID {id} not found.");
        await employeeRepo.DeleteAsync(id);
        await _UnitOfWork.CompleteAsync();
    }
    //------------------------------------------------------------------------
}
