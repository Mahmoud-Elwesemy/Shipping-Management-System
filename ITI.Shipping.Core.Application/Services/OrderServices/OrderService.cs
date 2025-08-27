using AutoMapper;
using ITI.Shipping.Core.Application.Abstraction.Order;
using ITI.Shipping.Core.Application.Abstraction.Order.Model;
using ITI.Shipping.Core.Application.Abstraction.OrderReport.Model;
using ITI.Shipping.Core.Application.Abstraction.Product.Model;
using ITI.Shipping.Core.Domin.Entities;
using ITI.Shipping.Core.Domin.Entities_Helper;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using ITI.Shipping.Core.Domin.UnitOfWork.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ITI.Shipping.Core.Application.Services.OrderServices
{
    internal class OrderService:IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IUnitOfWork unitOfWork , IMapper mapper ,UserManager<ApplicationUser> userManager,IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
           _httpContextAccessor = httpContextAccessor;
        }

        //---------------------------------------------------------------------------
        // Method to get the merchant name for each order
        private async Task<IEnumerable<OrderWithProductsDto>> GetMerchantName(IEnumerable<Order> orders)
        {
            var ordersDto = _mapper.Map<IEnumerable<OrderWithProductsDto>>(orders);
            foreach(var order in ordersDto)
            {
                var MerchantName = await _userManager.FindByIdAsync(order.MerchantName);
                order.MerchantName = MerchantName?.FullName ?? "اسم التاجر غير متاح";
            }
            return ordersDto;
        }
        //--------------------------------------------------------------------------

        // Get all orders
        public async Task<IEnumerable<OrderWithProductsDto>> GetOrdersAsync(Pramter pramter)
        {
            var orders = await _unitOfWork.GetOrderRepository().GetAllAsync(pramter);
            var ordersDto = await GetMerchantName(orders);
            return ordersDto;
        }
        //--------------------------------------------------------------------------
        // Get order by id
        public async Task<OrderWithProductsDto> GetOrderAsync(int id)
        {
            var findOrder = await _unitOfWork.GetOrderRepository().GetByIdAsync(id);

            if(findOrder == null || findOrder.IsDeleted)
            {
                return null;
            }

            var orderDto = _mapper.Map<OrderWithProductsDto>(findOrder);
            var MerchantName = await _userManager.FindByIdAsync(orderDto.MerchantName);
            orderDto.MerchantName = MerchantName!.FullName;
            return orderDto;
        }
        //--------------------------------------------------------------------------
        // Add new order And Calculate Shipping Cost And Create Order Report
        public async Task AddAsync(addOrderDto DTO)
        {
            decimal recalculatedOrderCost = DTO.Products.Sum(p => p.Price * p.Quantity);
            decimal recalculatedTotalWeight = DTO.Products.Sum(p => p.Weight * p.Quantity);         
            if(Math.Abs(recalculatedOrderCost - DTO.OrderCost) > 0.01m)
            {               
                DTO.OrderCost = recalculatedOrderCost;
            }
            if(Math.Abs(recalculatedTotalWeight - DTO.TotalWeight) > 0.01m)
            {
                DTO.TotalWeight = recalculatedTotalWeight;
            }
            decimal ShippingCost = 0;

           
            var IsOutOfCityShipping = DTO.IsOutOfCityShipping;


            var city = await _unitOfWork.GetRepository<CitySetting,int>().GetByIdAsync(DTO.City);
            var pickupShippingCost = city!.pickupShippingCost;
            decimal standardShippingCost = city!.StandardShippingCost;

            var Allweightsetting = await _unitOfWork.GetWeightSettingRepository().GetAllWeightSetting();
            var weightsetting = Allweightsetting.FirstOrDefault();
            decimal MaxWeight = weightsetting!.MaxWeight;
            decimal CostPerKG = weightsetting!.CostPerKg;

            var SpecialCityCost = await _unitOfWork.GetSpecialCityCostRepository()
                .GetCityCostByMarchantId(DTO.merchantId , DTO.City);

            
            if(SpecialCityCost != null)
            {
                if(DTO.TotalWeight > 0 && DTO.TotalWeight <= MaxWeight)
                {
                    if(IsOutOfCityShipping == true)
                        ShippingCost += SpecialCityCost.Price * 1.1m;
                    ShippingCost += SpecialCityCost.Price;
                }
                else if(DTO.TotalWeight > MaxWeight)
                {
                    decimal ExcessWeight = DTO.TotalWeight - MaxWeight;
                    if(IsOutOfCityShipping == true)
                        ShippingCost += SpecialCityCost.Price * 1.1m + (ExcessWeight * CostPerKG);
                    ShippingCost += SpecialCityCost.Price + (ExcessWeight * CostPerKG);
                }
            }
            else
            {
                if(DTO.TotalWeight > 0 && DTO.TotalWeight <= MaxWeight)
                {
                    if(IsOutOfCityShipping == true)
                        ShippingCost += standardShippingCost * 1.1m;
                    ShippingCost += standardShippingCost;
                }
                else if(DTO.TotalWeight > MaxWeight)
                {
                    decimal ExcessWeight = DTO.TotalWeight - MaxWeight;
                    if(IsOutOfCityShipping == true)
                        ShippingCost += standardShippingCost * 1.1m + (ExcessWeight * CostPerKG);
                    ShippingCost += standardShippingCost + (ExcessWeight * CostPerKG);
                }
            }

            var ShippingType = await _unitOfWork.GetRepository<ShippingType,int>().GetByIdAsync(DTO.ShippingId);
            if(ShippingType != null)
            {
                ShippingCost += ShippingType.BaseCost;
            }
            DTO.ShippingCost = ShippingCost;           
            
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            if(currentUser != null && await _userManager.IsInRoleAsync(currentUser,DefaultRole.Merchant))
            {
                DTO.status = OrderStatus.WaitingForConfirmation;
                DTO.merchantId = currentUser.Id;

            }
            else
            {
                DTO.status = OrderStatus.Pending;
                
            }
            var orderEntity = _mapper.Map<Order>(DTO);
            await _unitOfWork.GetOrderRepository().AddAsync(orderEntity);
            await _unitOfWork.CompleteAsync();

            // Retrieve the saved order to get the correct ID
            var savedOrder = await _unitOfWork.GetOrderRepository().GetByIdAsync(orderEntity.Id);

            // Create the order report When Add New order
            var orderReportDto = new OrderReportDTO
            {
                OrderId = savedOrder.Id, 
                ReportDate = DateTime.UtcNow
            };
            await _unitOfWork.GetOrderReportRepository().AddAsync(_mapper.Map<OrderReport>(orderReportDto));
            await _unitOfWork.CompleteAsync();    
        }
        //--------------------------------------------------------------------------
        // Get order for edit
        public async Task<updateOrderDto> GetOrderForEditAsync(int id)
        {
            var order = await _unitOfWork.GetOrderRepository().GetByIdAsync(id);
            

            if(order == null || order.IsDeleted)
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            var MerchantNameOfOrder = await _userManager.FindByIdAsync(order.MerchantId);
            return new updateOrderDto
            {
                Id = order.Id,
                OrderTypes = order.OrderTypes,
                IsOutOfCityShipping = order.IsOutOfCityShipping,
                ShippingId = (int) order.ShippingTypeId,
                PaymentType = order.PaymentType,
                Branch = (int) order.BranchId, 
                Region = (int) order.RegionId, 
                City = (int) order.CitySettingId,     
                TotalWeight = order.TotalWeight,
                merchantId = char.IsDigit(order.MerchantId[0])
                    ? MerchantNameOfOrder?.FullName ?? "اسم غير معروف"
                    : order.MerchantId,
                OrderCost = order.OrderCost,
                CustomerName = order.CustomerName,
                CustomerPhone1 = order.CustomerPhone1,
                CustomerAddress = order.CustomerAddress,
                CustomerEmail = order.CustomerEmail,
                Products = _mapper.Map<List<ProductDTO>>(order.Products)
            };
        }
        //--------------------------------------------------------------------------
        // Update order
        public async Task UpdateAsync(updateOrderDto DTO)
        {
            decimal recalculatedOrderCost = DTO.Products.Sum(p => p.Price * p.Quantity);
            decimal recalculatedTotalWeight = DTO.Products.Sum(p => p.Weight * p.Quantity);
            if(Math.Abs(recalculatedOrderCost - DTO.OrderCost) > 0.01m)
            {
                DTO.OrderCost = recalculatedOrderCost;
            }
            if(Math.Abs(recalculatedTotalWeight - DTO.TotalWeight) > 0.01m)
            {
                DTO.TotalWeight = recalculatedTotalWeight;
            }

            var OrderRepo = _unitOfWork.GetOrderRepository();
            var existingOrder = await OrderRepo.GetByIdAsync(DTO.Id);
            if(existingOrder == null)
                throw new KeyNotFoundException($"Order with ID {DTO.Id} not found.");
            _mapper.Map(DTO,existingOrder);
            OrderRepo.UpdateAsync(existingOrder);
            await _unitOfWork.CompleteAsync();
        }
        //--------------------------------------------------------------------------
        // Delete order
        public async Task DeleteAsync(int id)
        {
            var OrderRepo = _unitOfWork.GetOrderRepository();
            var existingOrder = await OrderRepo.GetByIdAsync(id);
            if(existingOrder == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            await OrderRepo.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
           
        }
        //--------------------------------------------------------------------------
        // Get all orders by status
        public async Task<IEnumerable<OrderWithProductsDto>> GetOrdersByStatus(OrderStatus status,Pramter pramter)
        {
            var orders = await _unitOfWork.GetOrderRepository().GetOrdersByStatus(status,pramter);

            var ordersDto = await GetMerchantName(orders);
            return ordersDto;
        }
        //--------------------------------------------------------------------------
        //  Get all waiting orders
        public async Task<IEnumerable<OrderWithProductsDto>> GetAllWatingOrder(Pramter pramter)
        {
          var WatingOrder =  await _unitOfWork.GetOrderRepository().GetAllWatingOrder(pramter);
            var WatingordersDto = await GetMerchantName(WatingOrder);
            return WatingordersDto;
        }
        //--------------------------------------------------------------------------
        // Change order status to pending
        public async Task ChangeOrderStatusToPending(int id)
        {
            await ChangeOrderStatus(id,OrderStatus.Pending);
        }
        //--------------------------------------------------------------------------
        // Change order status to Declined
        public async Task ChangeOrderStatusToDeclined(int id)
        {
            await ChangeOrderStatus(id,OrderStatus.Declined);
        }
        //--------------------------------------------------------------------------
        // Change order status
        public async Task ChangeOrderStatus(int id , OrderStatus orderStatus)
        {
                var Order = await _unitOfWork.GetOrderRepository().GetByIdAsync(id);
                Order!.Status = orderStatus;
                _unitOfWork.GetOrderRepository().UpdateAsync(Order);
                await _unitOfWork.CompleteAsync();
        }
        //--------------------------------------------------------------------------
        // Assign order to courier
        public async Task AssignOrderToCourier(int OrderId,string courierId)
        {
            if(string.IsNullOrWhiteSpace(courierId))
            {
                throw new ArgumentException("CourierId cannot be null or empty.");
            }
            var Order = await _unitOfWork.GetOrderRepository().GetByIdAsync(OrderId);
            if(Order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }
            Order!.CourierId = courierId;
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            Order!.EmployeeId = currentUser!.Id;
            Order.Status = OrderStatus.DeliveredToCourier;
            _unitOfWork.GetOrderRepository().UpdateAsync(Order);
            await _unitOfWork.CompleteAsync();
        }

        //--------------------------------------------------------------------------
        public async Task<IEnumerable<OrderWithProductsDto>> GetOrdersByMerchantIdAsync(string merchantId,Pramter pramter)
        {
            var Merchant =await _userManager.FindByIdAsync(merchantId);
            if(Merchant == null)
            {
                throw new KeyNotFoundException($"Merchant with ID {merchantId} not found.");
            }
            var orders =await _unitOfWork.GetOrderRepository().GetOrderByMerchantId(merchantId, pramter);
            if(orders == null)
            {
                throw new KeyNotFoundException($"No orders found for Merchant with ID {merchantId}.");
            }
            var ordersDto = _mapper.Map<IEnumerable<OrderWithProductsDto>>(orders);
            return ordersDto;



        }
        //--------------------------------------------------------------------------

        public async Task<IEnumerable<OrderWithProductsDto>> GetOrdersByCourierAsync(string courierId,Pramter pramter)
        {
            var Courier = await _userManager.FindByIdAsync(courierId);
            if(Courier == null)
            {
                throw new KeyNotFoundException($"Courier with ID {courierId} not found.");
            }
            var orders = await _unitOfWork.GetOrderRepository().GetOrderByCourierId(courierId,pramter);
            if(orders == null)
            {
                throw new KeyNotFoundException($"No orders found for Courier with ID {courierId}.");
            }
            var ordersDto = _mapper.Map<IEnumerable<OrderWithProductsDto>>(orders);
            return ordersDto;
        }
    }
}
