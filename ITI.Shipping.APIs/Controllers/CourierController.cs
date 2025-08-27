using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Application.Abstraction.Courier.DTO;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Shipping.APIs.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CourierController:ControllerBase
{
    private readonly IServiceManager _serviceManager;
    public CourierController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }
















    [HttpGet("GetCouriersByBranch")]
    public async Task<ActionResult<IEnumerable<CourierDTO>>> GetCouriersByBranch([FromQuery] int branchId)
    {
        var couriers = await _serviceManager.courierService.GetCourierByBranch(branchId);
        return Ok(couriers);
    }
    [HttpGet("GetCouriersByRegion")]
    public async Task<ActionResult<IEnumerable<CourierDTO>>> GetCouriersByRegion([FromQuery] int RegionId ,[FromQuery] Pramter pramter)
    {
        var couriers = await _serviceManager.courierService.GetCourierByRegion(RegionId ,pramter);
        Console.WriteLine($"Couriers returned: {couriers.Count()}");
        return Ok(couriers);
    }
}
