using ITI.Shipping.Core.Application.Abstraction.SpecialCityCost.Model;
using System.Text.Json.Serialization;

namespace ITI.Shipping.Core.Application.Abstraction.Merchant.Model;
public record MerchantDTO
{
    
    public string Id { get; set; }= string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
   
    public int BranchId { get; set; }
  
    public int RegionId { get; set; }
   
    public int CityId { get; set; }
    public string BranchNmae { get; set; } = string.Empty;
    public string RegionName { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;

    public IEnumerable<SpecialCityCostDTO>? SpecialCities { get; set; }

}
public record UpdateMerchantDTO
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public int RegionId { get; set; }  
    public int CityId { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public IEnumerable<SpecialCityCostToUpdateMerchantDTO>? SpecialCities { get; set; }
}
public record SpecialCityCostToUpdateMerchantDTO
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public int CitySettingId { get; set; }   
    public string MerchantId { get; set; } = string.Empty;
}
