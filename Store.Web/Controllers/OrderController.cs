using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities;
using Store.Services.Handle_Responses;
using Store.Services.Services.Cart.Dtos;
using Store.Services.Services.Orders;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Store.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        
        [HttpPost]
        public async Task <ActionResult<OrderDetailsDto>> CreateOrderAsync(OrderDto orderDto)
        {
            var order = await orderService.AddOrder(orderDto);

            if (order is null)
                return BadRequest(new Response(400, "Error While Creating Order"));

            return Ok(order);
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDetailsDto>>> GetAllOrdersForUserAsync()
        {
            var email = ClaimTypes.Email;
            if(email is not null)
            {
                var orders = await orderService.GetOrdersForCustomer(email);
                return Ok(orders);
            }
            return null;
        }
        [HttpGet]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderByIdAsync(Guid id)
        {
            var email = ClaimTypes.Email;
            if (email is not null)
            {
                var order = await orderService.GetOrder(id);
                return Ok(order);
            }
            return null;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllDeliveryMethods()
        {
            return Ok(await orderService.GetDeliveryMethods());
        }
    }
}
