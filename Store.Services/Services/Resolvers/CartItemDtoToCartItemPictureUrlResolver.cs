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
    public class CartItemDtoToCartItemPictureUrlResolver : IValueResolver<CartItemDto, CartItem, string>
    {
        private readonly IConfiguration _configuration;

        public CartItemDtoToCartItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(CartItemDto source, CartItem destination, string destMember, ResolutionContext context)
        {
            return _configuration["BaseUrl"] + source.PictureUrl;
        }
    }
}
