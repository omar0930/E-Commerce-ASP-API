using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specifications.OrderSpecs
{
    public class OrderWithItemSpecifications : BaseSpecifications<Order>
    {
        public OrderWithItemSpecifications(string BuyerEmail) : base(order => order.BuyerEmail == BuyerEmail || !String.IsNullOrEmpty(BuyerEmail))
        {
            AddInclude(order => order.DeliveryMethod);
            AddInclude(order => order.OrderItems);
            AddOrderByDesc(order => order.OrderDate);
        }
        public OrderWithItemSpecifications(Guid id) : base(order => order.Id == id)
        {
            AddInclude(order => order.DeliveryMethod);
            AddInclude(order => order.OrderItems);
            AddOrderByDesc(order => order.OrderDate);
        }
    }
}
