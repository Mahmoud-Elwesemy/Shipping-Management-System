using ITI.Shipping.Core.Domin.Entities;
namespace ITI.Shipping.Core.Domin.Repositories.contract;
// This Is A SpecialCityCost Repository Interface That Inharits From The Generic Repository Interface
public interface ISpecialCityCostRepository:IGenericRepository<SpecialCityCost,int>
{
    // This Is A Methods That Add A Range Of SpecialCityCost And Get City Cost By MerchantId
    Task AddRangeAsync(IEnumerable<SpecialCityCost> entities);
    Task<SpecialCityCost> GetCityCostByMarchantId(string MerchantId , int CityId);
    Task<IEnumerable<SpecialCityCost>> GetSpecialCityOfMerchant(string MerchatId);
}
