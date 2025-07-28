using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Repository.Cart;
using Store.Services.Services.Cart.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Services.Resolvers
{
    public class CartItemPictureUrlResolver : IValueResolver<CartItem, CartItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public CartItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(CartItem source, CartItemDto destination, string destMember, ResolutionContext context)
        {
            return _configuration["BaseUrl"] + source.PictureUrl;
        }
    }
}
