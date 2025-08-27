using ITI.Shipping.Core.Application.Abstraction.Courier.DTO;
using ITI.Shipping.Core.Application.Abstraction.Merchant.Model;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ITI.Shipping.Core.Application.Abstraction.Courier;
public interface ICourierService
{
    Task <IEnumerable<CourierDTO>> GetCourierByBranch(int OrderId);
    Task<IEnumerable<CourierDTO>> GetCourierByRegion(int OrderId,Pramter pramter);
    Task<IEnumerable<CourierDTO>> GetAllAsync(Pramter pramter);
    Task<CourierDTO?> GetCourierByIdAsync(string courierId);
    Task UpdateCourierAsync(CourierDTO CourierUpdate);
    Task DeleteCouriertAsync(string id);
}