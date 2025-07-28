using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specifications.OrderSpecs
{
    public class OrderWithPaymentIntentSpecifications : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpecifications(string PaymentIntentId) : base (order => order.PaymentIntentId == PaymentIntentId)
        {
        }
    }
}
