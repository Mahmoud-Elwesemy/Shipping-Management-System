namespace ITI.Shipping.Core.Application.Abstraction.Employee.Model;
public record EmployeeDTO
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public int BranchId { get; set; } 
    public string RoleId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

}
public record EmployeeUpdateDTO
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty; 
    public string PhoneNumber { get; set; } = string.Empty;
    public int BranchId { get; set; } 
    public string RoleId { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}
