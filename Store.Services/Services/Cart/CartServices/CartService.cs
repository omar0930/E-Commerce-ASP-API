using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Repository.Cart;
using Store.Repository.Cart.Interfaces;
using Store.Services.Services.Cart.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Services.Cart.CartServices
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public CartService(ICartRepository cartRepository, IConfiguration configuration, IMapper mapper)
        {
            _cartRepository = cartRepository;
            this.configuration = configuration;
            this.mapper = mapper;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _cartRepository.DeleteAsync(id);
        }
        public async Task<CartDto> GetAsync(Guid id)
        {
            var cart = await _cartRepository.GetAsync(id);

            if (cart == null)
            {
                return new CartDto();
            }

            var mappedCartItems = new List<CartItemDto>();
            foreach (var cartItem in cart.cartItems)
            {
                var mappedCartItem = new CartItemDto
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    ProductName = cartItem.ProductName,
                    BrandName = cartItem.BrandName,
                    CategoryName = cartItem.CategoryName,
                    Price = cartItem.Price,
                    PictureUrl = cartItem.PictureUrl,
                };

                mappedCartItems.Add(mappedCartItem);
            }

            var mappedCart = new CartDto
            {
                id = cart.id,
                shippingCost = cart.shippingCost.Value,
                cartItems = mappedCartItems,
                DeliveryMethodId = cart.DeliveryMethodId,
                //ClientSecret = cart.ClientSecret,
                //PaymentIntentId = cart.PaymentIntentId
            };

            return mappedCart;
        }
        public async Task<CartDto> UpdateAsync(CartDto cart)
        {
            if (cart.id is null)
                cart.id = GenerateRandomCartId();


            //var mappedCartItems = new List<CartItem>();
            //foreach (var cartItem in cart.cartItems)
            //{
            //    var mappedCartItem = new CartItem
            //    {
            //        ProductId = cartItem.ProductId,
            //        Quantity = cartItem.Quantity,
            //        ProductName = cartItem.ProductName,
            //        BrandName = cartItem.BrandName,
            //        CategoryName = cartItem.CategoryName,
            //        Price = cartItem.Price,
            //        PictureUrl = this.configuration["BaseUrl"] + "images/" + cartItem.PictureUrl
            //    };

            //    mappedCartItems.Add(mappedCartItem);
            //}

            var mappedCartItems = mapper.Map<List<CartItem>>(cart.cartItems);

            var mappedCart = new CustomerCart
            {
                id = cart.id,
                shippingCost = cart.shippingCost,
                cartItems = mappedCartItems,
                DeliveryMethodId = cart.DeliveryMethodId,
                PaymentIntentId = cart.PaymentIntentId,
                ClientSecret = cart.ClientSecret
            };

            var updatedCart = await _cartRepository.UpdateAsync(mappedCart);

            //var mappedCartItemsDto = new List<CartItemDto>();

            //foreach (var cartItem in cart.cartItems)
            //{
            //    var mappedCartItem = new CartItemDto
            //    {
            //        ProductId = cartItem.ProductId,
            //        Quantity = cartItem.Quantity,
            //        ProductName = cartItem.ProductName,
            //        BrandName = cartItem.BrandName,
            //        CategoryName = cartItem.CategoryName,
            //        Price = cartItem.Price,
            //        PictureUrl = this.configuration["BaseUrl"] + "images/" + cartItem.PictureUrl
            //    };

            //    mappedCartItemsDto.Add(mappedCartItem);
            //}

            //var mappedUpdatedCart = new CartDto
            //{
            //    id = updatedCart.id,
            //    shippingCost = updatedCart.shippingCost.Value,
            //    cartItems = mappedCartItemsDto,
            //    DeliveryMethodId = updatedCart.DeliveryMethodId,
            //    PaymentIntentId = updatedCart.PaymentIntentId,
            //    ClientSecret = updatedCart.ClientSecret
            //};

            var mappedUpdatedCart = mapper.Map<CartDto>(updatedCart);

            return mappedUpdatedCart;
        }
        private string GenerateRandomCartId()
        {
            return $"{Guid.NewGuid().ToString()}";
        }
    }
}
