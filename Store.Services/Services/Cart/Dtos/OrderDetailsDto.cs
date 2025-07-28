using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Store.Services.Services.Cart.Dtos
{
    public class OrderDetailsDto
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public ShippingAddressDto ShippingAddress { get; set; }
        public string DeliveryMethodName { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public OrderPaymentStatus OrderPaymentStatus { get; set; }
        public double ShippingPrice { get; set; }
        public Guid? CartId { get; set; }
        public double subTotal { get; set; }
        public double GetTotal { get; set; }
        public string? PaymentIntentId { get; set; }
    }
}
