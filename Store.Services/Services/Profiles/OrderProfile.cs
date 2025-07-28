using AutoMapper;
using Store.Data.Entities;
using Store.Services.Services.Cart.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Services.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<ShippingAddress, ShippingAddressDto>().ReverseMap();
            
            CreateMap<Order, OrderDetailsDto>()
                .ForMember(x => x.ShippingPrice, x => x.MapFrom(y => y.DeliveryMethod.Price))
                .ForMember(x => x.DeliveryMethodName, x => x.MapFrom(y => y.DeliveryMethod.ShortName)).ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(x => x.ProductName, x => x.MapFrom(y => y.ProductItem.Name))
                .ForMember(x => x.Price, x => x.MapFrom(y => y.ProductItem.Price))
                .ForMember(x => x.PictureUrl, x => x.MapFrom(y => y.ProductItem.PictureUrl)).ReverseMap();
        }
    }
}
