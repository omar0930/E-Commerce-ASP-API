using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Services.Cart.Dtos
{
    public class OrderDto
    {
        public Guid CartId { get; set; }
        public string BuyerEmail { get; set; }
        public ShippingAddressDto ShippingAddressDto { get; set; }
        public List<OrderItemDto> Items = new List<OrderItemDto>();
        [Required]
        public int DeliveryMethodId { get; set; }
    }
}
