using ITI.Shipping.Core.Domin.Entities;

namespace ITI.Shipping.Core.Domin.Repositories.contract;
public interface IBranchRepository:IGenericRepository<Branch,int>
{
    Task<IEnumerable<Branch>> GetBranchesByRegionIdAsync(int regionId);
    Task<IEnumerable<Branch>> GetBranchesByCitySettingIdAsync(int citySettingId);
}
