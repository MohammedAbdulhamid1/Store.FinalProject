using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Entites.Dtos.Orders;
using Store.Core.Entites.Orders;
using Store.Core.Repositories.Contract;
using Store.Core.Services.Contract;
using Store.FinalProject.Errors;
using System.Security.Claims;

namespace Store.FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitofwork unitofwork;

        public OrderController(IOrderService orderService, IMapper mapper,IUnitofwork unitofwork)
        {
            _orderService = orderService;
            _mapper = mapper;
            this.unitofwork = unitofwork;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto model)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null)
                return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var address = _mapper.Map<Address>(model.shipToAddress);

            var order = await _orderService.CreateOrderAsync(
                userEmail,
                model.BasketId,
                model.DeliveryMethodId,
                address
            );

            if (order is null)
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrdersForSpecificUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null)
                return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            var order = await _orderService.GetOrdersForSpecificUserAsync(userEmail);

            if (order is null)
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<IEnumerable<OrderToReturnDto>>(order));
        }
        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrdersForSpecificUser(int orderId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null)
                return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            var order = await _orderService.GetOrderByIdForSpecificUserAsync(userEmail,orderId);

            if (order is null)
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }
        
        [HttpGet("DeliveryMethod")]
        public async Task<IActionResult> GetDeliveryMethod()
        {
          
            var deliveryMethods = await unitofwork.Repositories<DeliveryMethod,int>().GetAllAsync();

            if (deliveryMethods is null)
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(deliveryMethods);
        }
    }
}
