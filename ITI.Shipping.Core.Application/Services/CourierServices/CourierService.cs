using AutoMapper;
using ITI.Shipping.Core.Application.Abstraction.Courier;
using ITI.Shipping.Core.Application.Abstraction.Courier.DTO;
using ITI.Shipping.Core.Application.Abstraction.Region.Model;
using ITI.Shipping.Core.Domin.Entities;
using ITI.Shipping.Core.Domin.Entities_Helper;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using ITI.Shipping.Core.Domin.UnitOfWork.Contract;
using Microsoft.AspNetCore.Identity;
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
        return _mapper.Map<IEnumerable<CourierDTO>> (await _unitOfWork.GetRepository<ApplicationUser,string>().GetAllAsync(pramter));
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
    //// Get Courier By Region
    //public async Task<IEnumerable<CourierDTO>> GetCourierByRegion(int OrderId,Pramter pramter)
    //{
    //    var order = await _unitOfWork.GetOrderRepository().GetByIdAsync(OrderId);
    //    var Courieres = await _userManager.GetUsersInRoleAsync(DefaultRole.Courier);
    //    var couriersInRegion = Courieres.Where(c => c.RegionId == order!.RegionId).ToList();


    //    //var region = await _unitOfWork.GetRepository<Region,int>().GetByIdAsync((int) order.RegionId);


    //    if(couriersInRegion.Count == 0)
    //    {
    //        var AllspecialRegion = await _unitOfWork.GetSpecialCourierRegionRepository().GetAllAsync(pramter);
    //        var specialCourierRegionbyRegion = AllspecialRegion.Where(r => r.RegionId == order!.RegionId);

    //        var couriersInSpecialRegion = Courieres.Where(c => specialCourierRegionbyRegion.Any(s => s.CourierId == c.Id));
    //        var couriersDto = _mapper.Map<IEnumerable<CourierDTO>>(couriersInSpecialRegion);
    //        return couriersDto;
    //    }
    //    else
    //    { 
    //    var couriersDtoInRegion = _mapper.Map<IEnumerable<CourierDTO>>(couriersInRegion);
    //    return couriersDtoInRegion;
    //    }
    //}

    // Get Courier By Region
    public async Task<IEnumerable<CourierDTO>> GetCourierByRegion(int OrderId,Pramter parameter)
    {
        // Retrieve the order by ID
        var order = await _unitOfWork.GetOrderRepository().GetByIdAsync(OrderId);
        if(order == null)
        {
            // Handle the case where the order is not found
            return Enumerable.Empty<CourierDTO>();
        }

        // Get all users in the Courier role
        var couriers = await _userManager.GetUsersInRoleAsync(DefaultRole.Courier);

        // Filter couriers by the order's region
        var couriersInRegion = couriers.Where(c => c.RegionId == order.RegionId).ToList();

        if(couriersInRegion.Count == 0)
        {
            // No couriers in the order's region; check special regions
            var specialRegions = await _unitOfWork.GetSpecialCourierRegionRepository().GetAllAsync(parameter);
            var relevantSpecialRegions = specialRegions.Where(r => r.RegionId == order.RegionId).ToList();

            // Extract unique courier IDs from relevant special regions
            var specialCourierIds = relevantSpecialRegions.Select(s => s.CourierId).Distinct().ToList();

            // Get couriers associated with the special regions
            var couriersInSpecialRegion = couriers.Where(c => specialCourierIds.Contains(c.Id)).ToList();

            return _mapper.Map<IEnumerable<CourierDTO>>(couriersInSpecialRegion);
        }

        // Return couriers in the order's region
        return _mapper.Map<IEnumerable<CourierDTO>>(couriersInRegion);
    }
}
