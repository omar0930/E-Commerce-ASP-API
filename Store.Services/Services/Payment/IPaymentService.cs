using Store.Services.Services.Cart.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Services.Payment
{
    public interface IPaymentService
    {
        Task<CartDto> CreateOrUpdatePaymentIntent(CartDto cartDto);
        Task<OrderDetailsDto> UpdateOrderPaymentSucceeded(string paymentIntentId);
        Task<OrderDetailsDto> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}
