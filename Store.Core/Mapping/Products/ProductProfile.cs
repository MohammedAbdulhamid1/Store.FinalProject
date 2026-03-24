using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Core.Entites;
using Store.Core.Entites.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Mapping.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile(IConfiguration configuration)
        {
            CreateMap<Product, ProductDto>()
    .ForMember(d => d.BrandName, options => options.MapFrom(s => s.Brand != null ? s.Brand.Name : null))
    .ForMember(d => d.TypeName, options => options.MapFrom(s => s.Type != null ? s.Type.Name : null))
           .ForMember(d => d.PictureUrl, options => options.MapFrom(s => $"{configuration["BASEURL"]}/{s.PictureUrl}"));
            CreateMap<ProductBrand, ProductBrandDto>();
            CreateMap<ProductType, ProductTypeDto>();

        }
    }
}
