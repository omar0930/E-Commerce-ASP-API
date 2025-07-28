using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specifications;
using Store.Repository.Specifications.OrderSpecs;
using Store.Services.Services.Cart.CartServices;
using Store.Services.Services.Cart.Dtos;
using Store.Services.Services.Orders;
using Store.Services.Services.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Services.Ordersz
{
    public class OrderService : IOrderService
    {
        private readonly ICartService cartService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPaymentService paymentService;
        private readonly IConfiguration configurtion;

        public OrderService(ICartService cartService, IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService, IConfiguration configurtion)
        {
            this.cartService = cartService;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.paymentService = paymentService;
            this.configurtion = configurtion;
        }
        public async Task<OrderDetailsDto> AddOrder(OrderDto order)
        {
            var Cart = await cartService.GetAsync(order.CartId);

            if (Cart == null)
            {
                throw new Exception($"Cart With Id {Cart.id} Not Exist");
            }

            var OrderItems = new List<OrderItemDto>();

            foreach (var CartItem in Cart.cartItems)
            {
                var Product_Specs = new ProductWithSpecifications(CartItem.ProductId);
                var product = await unitOfWork.Repository<Product, int>().GetByIdWithSpecifications(Product_Specs);

                if (product is null)
                    throw new Exception($"Product With Id {CartItem.ProductId} Not Exist");

                var itemOrdered = new ProductItem
                {
                    Name = product.Name,
                    PictureUrl = this.configurtion["BaseUrl"] + product.PictureUrl,
                    Price = product.Price
                };

                var orderItem = new OrderItem
                {
                    Price = product.Price,
                    Quantity = CartItem.Quantity,
                    ProductItem = itemOrdered
                };

                //var mappedOrderItem = new OrderItemDto
                //{
                //    ProductName = orderItem.ProductItem.Name,
                //    Price = orderItem.Price,
                //    Quantity = orderItem.Quantity
                //};
                var mappedOrderItem = mapper.Map<OrderItemDto>(orderItem);

                OrderItems.Add(mappedOrderItem);
            }

            var DeliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetById(order.DeliveryMethodId);

            if (DeliveryMethod is null)
                throw new Exception($"Delivery Method With Id {order.DeliveryMethodId} Not Exist");

            var SubTotal = OrderItems.Sum(oi => oi.Quantity * oi.Price);

            #region Payment
           
            var specs = new OrderWithPaymentIntentSpecifications(Cart.PaymentIntentId);

            var existingOrder = await unitOfWork.Repository<Order, Guid>().GetByIdWithSpecifications(specs);

            if (existingOrder is null)
                await paymentService.CreateOrUpdatePaymentIntent(Cart);

            #endregion

            //var mappedShippedAddress = mapper.Map<ShippingAddressDto>(order.ShippingAddressDto);

            var shippedAddress = mapper.Map<ShippingAddress>(order.ShippingAddressDto);

            var orderItems = mapper.Map<List<OrderItem>>(OrderItems);
            var Order = new Order
            {
                OrderItems = orderItems,
                subTotal = SubTotal,
                DeliveryMethodId = DeliveryMethod.Id,
                ShippingAddress = shippedAddress,
                OrderPaymentStatus = OrderPaymentStatus.PaymentSucceeded,
                OrderStatus = OrderStatus.Pending,
                OrderDate = DateTime.Now,
                BuyerEmail = order.BuyerEmail,
                CartId = order.CartId,
                PaymentIntentId = Cart.PaymentIntentId,
                BuyerName = ClaimTypes.GivenName
            };

            await unitOfWork.Repository<Order, Guid>().AddAsync(Order);
            await unitOfWork.CompleteAsync();

            var mappedOrder = mapper.Map<OrderDetailsDto>(Order);

            return mappedOrder;
        }
        public async Task<bool> DeleteOrder(Guid id)
        {
            var Order = await unitOfWork.Repository<Order, Guid>().GetById(id);

            if (Order == null)
                throw new Exception($"Order With Id {id} Not Exist");

            unitOfWork.Repository<Order, Guid>().DeleteAsync(Order);
            await unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethods()
        {
            var DeliveryMethods = unitOfWork.Repository<DeliveryMethod, int>().GetAll();

            return await DeliveryMethods.ToListAsync();
        }

        public async Task<OrderDetailsDto> GetOrder(Guid id)
        {
            var Order = await unitOfWork.Repository<Order, Guid>().GetById(id);

            if (Order == null)
                throw new Exception($"Order With Id {id} Not Exist");

            var OrderItems = new List<OrderItemDto>();
            for (int i = 0; i < Order.OrderItems.Count; i++)
            {
                var mappedOrderItem = new OrderItemDto
                {
                    ProductName = Order.OrderItems[i].ProductItem.Name,
                    Price = Order.OrderItems[i].Price,
                    Quantity = Order.OrderItems[i].Quantity,
                    PictureUrl = Order.OrderItems[i].ProductItem.PictureUrl,
                    OrderId = Order.Id,
                };
                OrderItems.Add(mappedOrderItem);
            }

            var mappedShippedAddress = new ShippingAddressDto
            {
                FirstName = Order.ShippingAddress.FirstName,
                City = Order.ShippingAddress.City,
                State = Order.ShippingAddress.State,
                Street = Order.ShippingAddress.Street,
                ZipCode = Order.ShippingAddress.ZipCode,
                LastName = Order.ShippingAddress.LastName,
            };

            var DeliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetById((int)Order.DeliveryMethodId);

            var mappedOrder = new OrderDetailsDto
            {
                OrderItems = OrderItems,
                subTotal = Order.subTotal,
                DeliveryMethodName = DeliveryMethod.ShortName,
                ShippingAddress = mappedShippedAddress,
                OrderPaymentStatus = OrderPaymentStatus.PaymentSucceeded,
                OrderStatus = Order.OrderStatus,
                OrderDate = Order.OrderDate,
                BuyerEmail = Order.BuyerEmail,
                CartId = Order.CartId,
                ShippingPrice = DeliveryMethod.Price,
                Id = Order.Id,
            };

            return mappedOrder;
        }

        public async Task<IReadOnlyList<OrderDto>> GetOrders()
        {
            var Orders = unitOfWork.Repository<Order, Guid>().GetAll();

            var mappedOrders = mapper.Map<IReadOnlyList<OrderDto>>(Orders);

            return mappedOrders;
        }

        public async Task<IReadOnlyList<OrderDetailsDto>> GetOrdersForCustomer(string BuyerEmail)
        {
            var specs = new OrderWithItemSpecifications(BuyerEmail);

            var orders = await unitOfWork.Repository<Order, Guid>().GetAllWithSpecifications(specs);

            if (!orders.Any())
                throw new Exception("You Haven't Created Any Orders Yet");

            var mappedOrderList = new List<OrderDetailsDto>();

            foreach (var order in orders)
            {
                var OrderItems = new List<OrderItemDto>();
                for (int i = 0; i < order.OrderItems.Count; i++)
                {
                    var mappedOrderItem = new OrderItemDto
                    {
                        ProductName = order.OrderItems[i].ProductItem.Name,
                        Price = order.OrderItems[i].Price,
                        Quantity = order.OrderItems[i].Quantity,
                        PictureUrl = order.OrderItems[i].ProductItem.PictureUrl,
                        OrderId = order.Id,
                    };
                    OrderItems.Add(mappedOrderItem);
                }

                var mappedShippedAddress = new ShippingAddressDto
                {
                    FirstName = order.ShippingAddress.FirstName,
                    City = order.ShippingAddress.City,
                    State = order.ShippingAddress.State,
                    Street = order.ShippingAddress.Street,
                    ZipCode = order.ShippingAddress.ZipCode,
                    LastName = order.ShippingAddress.LastName,
                };

                var DeliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetById((int)order.DeliveryMethodId);

                var mappedOrderDetails = new OrderDetailsDto
                {
                    OrderItems = OrderItems,
                    subTotal = order.subTotal,
                    DeliveryMethodName = DeliveryMethod.ShortName,
                    ShippingAddress = mappedShippedAddress,
                    OrderPaymentStatus = OrderPaymentStatus.PaymentSucceeded,
                    OrderStatus = order.OrderStatus,
                    OrderDate = order.OrderDate,
                    BuyerEmail = order.BuyerEmail,
                    CartId = order.CartId,
                    ShippingPrice = DeliveryMethod.Price,
                    Id = order.Id,
                };

                mappedOrderList.Add(mappedOrderDetails);
            }

            return mappedOrderList;
        }
        public Task<OrderDetailsDto> UpdateOrder(OrderDto order)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateOrderStatus(Guid id, OrderStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
