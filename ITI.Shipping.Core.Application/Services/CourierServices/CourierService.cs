using AutoMapper;
using ITI.Shipping.Core.Application.Abstraction.Courier;
using ITI.Shipping.Core.Application.Abstraction.Courier.DTO;
using ITI.Shipping.Core.Application.Abstraction.Region.Model;
using ITI.Shipping.Core.Application.Abstraction.SpecialCourierRegion.Model;
using ITI.Shipping.Core.Domin.Entities;
using ITI.Shipping.Core.Domin.Entities_Helper;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using ITI.Shipping.Core.Domin.UnitOfWork.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Shipping.Core.Application.Services.CourierServices;
internal class CourierService:ICourierService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public CourierService(IUnitOfWork unitOfWork,IMapper mapper,UserManager<ApplicationUser> userManager)
    {
       _unitOfWork = unitOfWork;
       _mapper = mapper;
       _userManager = userManager;
    }

    // Get All Courier 
    public async Task<IEnumerable<CourierDTO>> GetAllAsync(Pramter pramter)
    {
        var couriers = await _userManager.GetUsersInRoleAsync(DefaultRole.Courier);

        var query = couriers
          .OrderByDescending(c => c.CreatedAt)
          .AsQueryable();
        if(pramter.PageNumber.HasValue && pramter.PageSize.HasValue)
        {
            query = query
                .Skip(((pramter.PageNumber ?? 1) - 1) * (pramter.PageSize ?? 10))
                .Take(pramter.PageSize ?? 10);
        }
        var Allcouriers = await query.ToListAsync();

        return _mapper.Map<IEnumerable<CourierDTO>>(Allcouriers);
    }
    // Get Courier By Id
    public async Task<CourierDTO?> GetCourierByIdAsync(string courierId)
    {
        var courier = await _userManager.FindByIdAsync(courierId);
        if(courier == null || !(await _userManager.IsInRoleAsync(courier,DefaultRole.Courier)))
            return null;
        var courierDto = _mapper.Map<CourierDTO>(courier);

        return courierDto;
    }
    // Get Courier By Branch
    public async Task<IEnumerable<CourierDTO>> GetCourierByBranch(int OrderId)
    {
        var order = await _unitOfWork.GetOrderRepository().GetByIdAsync(OrderId);
        var Courieres = await _userManager.GetUsersInRoleAsync(DefaultRole.Courier);
        var couriersInBranch  = Courieres.Where(c => c.BranchId == order!.BranchId);
        var couriersDto = _mapper.Map<IEnumerable<CourierDTO>>(couriersInBranch);
        return couriersDto;
    }
    // Get Courier By Region
    public async Task<IEnumerable<CourierDTO>> GetCourierByRegion(int OrderId,Pramter parameter)
    {
        var order = await _unitOfWork.GetOrderRepository().GetByIdAsync(OrderId);
        if(order == null)
        {
            return Enumerable.Empty<CourierDTO>();
        }
        var couriers = await _userManager.GetUsersInRoleAsync(DefaultRole.Courier);
        var couriersInRegion = couriers.Where(c => c.RegionId == order.RegionId).ToList();

        if(couriersInRegion.Count == 0)
        {
            var specialRegions = await _unitOfWork.GetSpecialCourierRegionRepository().GetAllAsync(parameter);
            var relevantSpecialRegions = specialRegions.Where(r => r.RegionId == order.RegionId).ToList();
            var specialCourierIds = relevantSpecialRegions.Select(s => s.CourierId).Distinct().ToList();
            var couriersInSpecialRegion = couriers.Where(c => specialCourierIds.Contains(c.Id)).ToList();

            return _mapper.Map<IEnumerable<CourierDTO>>(couriersInSpecialRegion);
        }
        return _mapper.Map<IEnumerable<CourierDTO>>(couriersInRegion);
    }

    public async Task UpdateCourierAsync(CourierDTO CourierUpdate)
    {
        var courier = await _userManager.FindByIdAsync(CourierUpdate.Id);
        if(courier == null || !(await _userManager.IsInRoleAsync(courier,DefaultRole.Courier)))
            throw new Exception("Courier not found");
        courier.FullName = CourierUpdate.FullName;
        courier.PhoneNumber = CourierUpdate.PhoneNumber;
        courier.Address = CourierUpdate.Address;
        courier.BranchId = CourierUpdate.BranchId;
        courier.DeductionTypes = CourierUpdate.DeductionTypes;
        courier.DeductionCompanyFromOrder = CourierUpdate.DeductionCompanyFromOrder;
        courier.Email = CourierUpdate.Email;
        courier.SpecialCourierRegion = _mapper.Map<List<SpecialCourierRegion>>(CourierUpdate.SpecialCourierRegionOfCourier);


    }
    public Task DeleteCouriertAsync(string id)
    {
        throw new NotImplementedException();
    }
}
