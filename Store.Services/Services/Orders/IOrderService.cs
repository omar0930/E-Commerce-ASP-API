using Store.Data.Entities;
using Store.Services.Services.Cart.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Services.Orders
{
    public interface IOrderService
    {
        Task<OrderDetailsDto> AddOrder(OrderDto order);
        Task<IReadOnlyList<OrderDto>> GetOrders();
        Task<OrderDetailsDto> GetOrder(Guid id);
        public Task<bool> DeleteOrder(Guid id);
        Task<IReadOnlyList<OrderDetailsDto>> GetOrdersForCustomer(string BuyerEmail);
        Task<OrderDetailsDto> UpdateOrder(OrderDto order);
        Task<bool> UpdateOrderStatus(Guid id, OrderStatus status);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethods();
    }
}
