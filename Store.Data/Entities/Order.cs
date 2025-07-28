using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Entities
{
    public class Order : BaseEntity<Guid>
    {
        public string BuyerEmail { get; set; }
        public string BuyerName { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public ShippingAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public int? DeliveryMethodId { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public OrderPaymentStatus OrderPaymentStatus { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public Guid? CartId { get; set; }
        public double subTotal { get; set; }
        public double GetTotal()
            => subTotal + DeliveryMethod.Price;
        public string? PaymentIntentId { get; set; }
    }
}
