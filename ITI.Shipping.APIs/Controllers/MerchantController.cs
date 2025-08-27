using ITI.Shipping.APIs.Filters;
using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Application.Abstraction.Merchant.Model;
using ITI.Shipping.Core.Domin.Entities_Helper;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Shipping.APIs.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MerchantController(IServiceManager serviceManager):ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;

    [HttpGet("GetMerchant")]
    [HasPermission(Permissions.ViewMerchants,Permissions.UpdateMerchants,Permissions.AddOrders,Permissions.UpdateOrders)]
    public async Task<ActionResult<IEnumerable<MerchantDTO>>> GetAllMerchant()
    {
        var merchants = await _serviceManager.merchantService.GetAllMerchantAsync();
        return Ok(merchants);
    }
    [HttpGet("GetMerchant/{id}")]
    [HasPermission(Permissions.ViewMerchants,Permissions.UpdateMerchants,Permissions.AddOrders,Permissions.UpdateOrders)]
    public async Task<ActionResult<MerchantDTO>> GetMerchantById(string id)
    {
        var merchant = await _serviceManager.merchantService.GetMerchantByIdAsync(id);
        return Ok(merchant);
    }

    [HttpPut("UpdateMerchant")]
    [HasPermission(Permissions.UpdateMerchants)]
    public async Task<IActionResult> UpdateMerchant([FromBody] UpdateMerchantDTO merchantUpdate)
    {
        await _serviceManager.merchantService.UpdateMerchantAsync(merchantUpdate);
        return NoContent();
    }

    [HttpDelete("DeleteMerchant/{id}")]
    [HasPermission(Permissions.DeleteMerchants)]
    public async Task<IActionResult> DeleteMerchant(string id)
    {
        await _serviceManager.merchantService.DeleteMerchantAsync(id);
        return NoContent();
    }
}
