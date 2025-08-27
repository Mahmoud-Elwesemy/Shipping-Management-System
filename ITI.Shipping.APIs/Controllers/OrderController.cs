using ITI.Shipping.APIs.Filters;
using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Application.Abstraction.Order.Model;
using ITI.Shipping.Core.Domin.Entities_Helper;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using ITI.Shipping.Core.Domin.ResponseHelper;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Shipping.APIs.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrderController:ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public OrderController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet] // Get : /api/Order
    [HasPermission(Permissions.ViewOrders)]
    public async Task<ActionResult<IEnumerable<OrderWithProductsDto>>> GetAllOrder([FromQuery] Pramter pramter)
    {
        var Orders = await _serviceManager.orderService.GetOrdersAsync(pramter);
        return Ok(Orders);
    }

    [HttpGet("GetAllOrdersByStatus")] // Get : /api/Order/GetAllOrdersByStatus
    [HasPermission(Permissions.ViewOrders)]

    public async Task<IActionResult> GetAllOrdersByStatus(OrderStatus status,[FromQuery] Pramter pramter)
    {
        try
        {
            var orders = await _serviceManager.orderService.GetOrdersByStatus(status,pramter);
            if(orders.ToList().Count == 0)
                return NotFound(new ResponseAPI(StatusCodes.Status404NotFound,"No orders found"));
            return Ok(orders);
        }
        catch
        {
            return BadRequest(new ResponseAPI(StatusCodes.Status400BadRequest));
        }
    }

    [HttpGet("GetAllOrdersByMerchantId")] // Get : /api/Order/GetAllOrdersByMerchantId
    [HasPermission(Permissions.ViewOrders)]
    public async Task<ActionResult> GetAllOrdersByMerchantId(string merchantId ,[FromQuery] Pramter pramter)
    {
        try
        {
            var orders = await _serviceManager.orderService.GetOrdersByMerchantIdAsync(merchantId,pramter);
            if(orders.ToList().Count == 0)
                return NotFound(new ResponseAPI(StatusCodes.Status404NotFound,"No orders found"));
            return Ok(orders);
        }
        catch
        {
            return BadRequest(new ResponseAPI(StatusCodes.Status400BadRequest));
        }
    }

    [HttpGet("GetAllOrdersByCourierId")] // Get : /api/Order/GetAllOrdersByCourierId
    [HasPermission(Permissions.ViewOrders)]
    public async Task<ActionResult> GetAllOrdersByCourierId(string courierId,[FromQuery] Pramter pramter)
    {
        try
        {
            var orders = await _serviceManager.orderService.GetOrdersByCourierAsync(courierId,pramter);
            if(orders.ToList().Count == 0)
                return NotFound(new ResponseAPI(StatusCodes.Status404NotFound,"No orders found"));
            return Ok(orders);
        }
        catch
        {
            return BadRequest(new ResponseAPI(StatusCodes.Status400BadRequest));
        }
    }




    [HttpGet("{id}")] // Get : /api/Order/id
    [HasPermission(Permissions.ViewOrders)]
    public async Task<ActionResult<OrderWithProductsDto>> GetOrder(int id)
    {
        var Order = await _serviceManager.orderService.GetOrderAsync(id);
        if(Order == null)
            return NotFound();
        return Ok(Order);
    }

    [HttpPost] // Post : /api/Order
    [HasPermission(Permissions.AddOrders)]
    public async Task<ActionResult<addOrderDto>> AddOrder(addOrderDto DTO)
    {
        if(DTO == null)
            return BadRequest("Invalid Order data");
        await _serviceManager.orderService.AddAsync(DTO);
        return Ok();
    }

    [HttpGet("GetOrderForEdit/{id}")]
    [HasPermission(Permissions.UpdateOrders)]
    public async Task<ActionResult<updateOrderDto>> GetOrderForEdit(int id)
    {
        var order = await _serviceManager.orderService.GetOrderForEditAsync(id);
        if(order == null)
            return NotFound();
        return Ok(order);
    }

    [HttpPut("{id}")] // Put : /api/Order/id
    [HasPermission(Permissions.UpdateOrders)]
    public async Task<ActionResult> UpdateOrder(int id,[FromBody] updateOrderDto DTO)
    {
        if(DTO == null || id != DTO.Id)
            return BadRequest("Invalid branch data.");
        try
        {
            await _serviceManager.orderService.UpdateAsync(DTO);
            return NoContent(); // 204 No Content (successful update)
        }
        catch(KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")] // Delete : /api/Order/id
    [HasPermission(Permissions.DeleteOrders)]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        try
        {
            await _serviceManager.orderService.DeleteAsync(id);
            return NoContent(); // 204 No Content (successful deletion)
        }
        catch(KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("GetAllWaitingOrders")] // Get : /api/Order/GetAllWaitingOrders
    [HasPermission(Permissions.ViewOrders)]
    public async Task<ActionResult<IEnumerable<OrderWithProductsDto>>> GetAllWatingOrder([FromQuery] Pramter pramter)
    {
        var Orders = await _serviceManager.orderService.GetAllWatingOrder(pramter);
        return Ok(Orders);
    }

    [HttpPost("ChangeOrderStatusToPending/{id}")] // Post : /api/Order/ChangeOrderStatusToPending/id
    [HasPermission(Permissions.UpdateOrders)]
    public async Task<IActionResult> ChangeOrderStatusTOPending(int id)
    {
        try
        {
            await _serviceManager.orderService.ChangeOrderStatusToPending(id);
            return NoContent(); // 204 No Content (successful update)
        }
        catch(KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpPost("ChangeOrderStatusToDeclined/{id}")] // Post : /api/Order/ChangeOrderStatusToDeclined/id
    [HasPermission(Permissions.UpdateOrders)]
    public async Task<IActionResult> ChangeOrderStatusToDeclined(int id)
    {
        try
        {
            await _serviceManager.orderService.ChangeOrderStatusToDeclined(id);
            return NoContent(); // 204 No Content (successful update)
        }
        catch
        {
            return NotFound(new ResponseAPI(StatusCodes.Status404NotFound));
        }
    }
    [HttpPost ("UpdateStatus/{id}")]
    public async Task<IActionResult> UpdateStatus(int id,OrderStatus status)
    {
        try
        {
            await _serviceManager.orderService.ChangeOrderStatus(id,status);
            return NoContent(); // 204 No Content (successful update)
        }
        catch
        {
            return NotFound(new ResponseAPI(StatusCodes.Status404NotFound));
        }
    }
    [HttpPost("AssignOrderToCourier/{OrderId}/{courierId}")] // Post : /api/Order/AssignOrderToCourier/OrderId/courierId
    [HasPermission(Permissions.UpdateOrders)]
    public async Task<IActionResult> AssignOrderToCourier(int OrderId,string courierId)
    {
        try
        {
            if(string.IsNullOrWhiteSpace(courierId))
            {
                return BadRequest(new ResponseAPI(StatusCodes.Status400BadRequest,"CourierId is required."));
            }
            await _serviceManager.orderService.AssignOrderToCourier(OrderId,courierId);
            return NoContent(); // 204 No Content (successful update)
        }
        catch(KeyNotFoundException)
        {
            return NotFound(new ResponseAPI(StatusCodes.Status404NotFound,"Order not found."));
        }
        catch(Exception ex)
        {
            return BadRequest(new ResponseAPI(StatusCodes.Status400BadRequest,ex.Message));
        }
    }

}
