using ITI.Shipping.Core.Domin.Repositories.contract;
namespace ITI.Shipping.Core.Domin.UnitOfWork.Contract
{
    // This Is A Unit Of Work Interface That Contains The Repositories
    public interface IUnitOfWork:IAsyncDisposable
    {
        // This Is A Method That Get The Generic Repository And All Repositories
        IGenericRepository<T,Tkey> GetRepository<T, Tkey>()
            where T : class where Tkey : IEquatable<Tkey>;
        ICityRepository GetCityRepository();
        ISpecialCourierRegionRepository GetSpecialCourierRegionRepository();
        ISpecialCityCostRepository GetSpecialCityCostRepository();
        IOrderRepository GetOrderRepository();
        IOrderReportRepository GetOrderReportRepository();
        Task<int> CompleteAsync();
        IWeightSettingRepository GetWeightSettingRepository();
        IEmployeeRepository GetEmployeeRepository();
        IMerchantRepository GetMerchantRepository();
        IBranchRepository GetBranchesRepository();
        IProductRepository GetProductRepository();
    }
}