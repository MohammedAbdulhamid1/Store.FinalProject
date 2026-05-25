using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Core.Entites.Dtos.Orders;
using Store.Core.Entites.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Mapping.Orders
{
    public class OrderProfile : Profile
    {
        public OrderProfile(IConfiguration configuration)
        {
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.Id,
                    options => options.MapFrom(s => s.Id))
                .ForMember(d => d.DeliveryMethod,
                    options => options.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost,
                    options => options.MapFrom(s => s.DeliveryMethod.Cost))
                .ForMember(d => d.Status,
                    options => options.MapFrom(s => s.Status.ToString()));

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId,
                    options => options.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName,
                    options => options.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl,
                    options => options.MapFrom(s => $"{configuration["BASEURL"]}{s.Product.PictureUrl}"));
        }
    }

    }
