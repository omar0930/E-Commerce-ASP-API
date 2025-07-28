using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.Cart.CartServices;
using Store.Services.Services.Cart.Dtos;

namespace Store.Web.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpDelete("{id}")]
        public async Task<bool> DeleteAsync(Guid id)
        {
            await _cartService.DeleteAsync(id);
            return true;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CartDto>> GetAsync(Guid id)
        {
            return await _cartService.GetAsync(id);
        }
        [HttpPost]
        public Task<CartDto> UpdateAsync(CartDto cart)
        {
            return _cartService.UpdateAsync(cart);
        }
    }
}
