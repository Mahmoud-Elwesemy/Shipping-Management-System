using ITI.Shipping.Core.Application.Abstraction.SpecialCourierRegion.Model;
using ITI.Shipping.Core.Domin.Entities_Helper;
namespace ITI.Shipping.Core.Application.Abstraction.Courier.DTO;
public record CourierDTO
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public string branchName { get; set; } = string.Empty;
    public DeductionTypes? DeductionTypes { get; set; }
    public decimal? DeductionCompanyFromOrder { get; set; }
    public  List<SpecialCourierRegionDTO> SpecialCourierRegionOfCourier { get; set; } = [];
}

