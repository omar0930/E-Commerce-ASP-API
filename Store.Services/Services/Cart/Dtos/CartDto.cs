using Store.Repository.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Services.Cart.Dtos
{
    public class CartDto
    {
        public string? id { get; set; }
        public double shippingCost { get; set; }
        public List<CartItemDto> cartItems { get; set; } = new List<CartItemDto>();
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
