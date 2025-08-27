using ITI.Shipping.Core.Application.Abstraction.Employee.Model;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ITI.Shipping.Core.Application.Abstraction.Employee;
public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync(Pramter pramter);
    Task<EmployeeDTO> GetEmployeeByIdAsync(string id);
    Task UpdateAsync(EmployeeUpdateDTO DTO);
    Task DeleteAsync(string id);
}
