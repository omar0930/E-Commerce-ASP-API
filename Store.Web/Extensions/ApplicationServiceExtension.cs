using Microsoft.AspNetCore.Mvc;
using Store.Repository.Interfaces;
using Store.Repository;
using Store.Services.Services;
using Store.Services.Handle_Responses;
using Store.Services.Services.CacheServices;
using Store.Repository.Cart.Interfaces;
using Store.Repository.Cart;
using Store.Services.Services.Cart.CartServices;
using Store.Services.Services.TokenServices;
using Store.Services.Services.UserServices;
using Store.Services.Services.Orders;
using Store.Services.Services.Ordersz;
using AutoMapper;
using Store.Services.Services.Payment;
using Store.Services.Services.Profiles;
using Store.Services.Services.Resolvers;

namespace Store.Web.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<CartItemPictureUrlResolver>();
            services.AddAutoMapper(typeof(OrderProfile), typeof(CartProfile));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(model => model.Value.Errors.Count > 0).SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).ToList();
                    var errorResponse = new ValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });
            return services;
        }
    }
}
