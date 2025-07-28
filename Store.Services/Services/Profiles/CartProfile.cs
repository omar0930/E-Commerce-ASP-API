using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Repository.Cart;
using Store.Services.Services.Cart.Dtos;
using Store.Services.Services.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Services.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<CartItemPictureUrlResolver>())
            .ReverseMap()
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<CartItemDtoToCartItemPictureUrlResolver>());

            CreateMap<CustomerCart, CartDto>().ReverseMap();
        }
    }

}
