using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Cart
{
    public class CustomerCart
    {
        public string? id { get; set; }

        public double ? shippingCost { get; set; }

        public List<CartItem>? cartItems { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }

        public int? DeliveryMethodId { get; set; }
    }
}
