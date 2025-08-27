using ITI.Shipping.Core.Domin.Entities;
using ITI.Shipping.Core.Domin.Repositories.contract;
using ITI.Shipping.Infrastructure.Presistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.Shipping.Infrastructure.Presistence.Repositories;
public class BranchRepository:GenericRepository<Branch,int>, IBranchRepository
{
    private readonly ApplicationContext _applicationContext;
    // This Is A Branch Repository Class That Implements The IBranchRepository Interface
    public BranchRepository(ApplicationContext applicationContext) : base(applicationContext)
    {
        _applicationContext = applicationContext;
    }
    // This Method Is Used To Get All Branches By Region Id
    public async Task<IEnumerable<Branch>> GetBranchesByRegionIdAsync(int regionId)
    {
        return await _applicationContext.Branches
            .Where(b => b.RegionId == regionId)
            .ToListAsync();
    }
    // This Method Is Used To Get All Branches By City Setting Id
    public async Task<IEnumerable<Branch>> GetBranchesByCitySettingIdAsync(int citySettingId)
    {
        return await _applicationContext.Branches
            .Where(b => b.CitySettingId == citySettingId)
            .ToListAsync();
    }
}
