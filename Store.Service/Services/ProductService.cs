using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Entites;
using Store.Core.Entites.Dtos;
using Store.Core.Helper;
using Store.Core.Repositories.Contract;
using Store.Core.Services.Contract;
using Store.Core.Specification;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitofwork unitofwork;
        private readonly IMapper mapper;

        public ProductService(IUnitofwork unitofwork,IMapper mapper)
        {
            this.unitofwork = unitofwork;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<ProductBrandDto>> GetAllBrandsAsync()
        
        =>  mapper.Map<IEnumerable<ProductBrandDto>>(await unitofwork.Repositories<ProductBrand, int>().GetAllAsync());




        public async Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpecParams)
        {
            var spec = new ProductSpecification(productSpecParams);
            var products = await unitofwork.Repositories<Product, int>().GetAllWithSpecAsync(spec);
            var mapping=  mapper.Map<IEnumerable<ProductDto>>(products);
            var countspec = new ProductWithCountSpecification(productSpecParams);
            var count = await unitofwork.Repositories<Product, int>().GetCountAsync(countspec);
            return new PaginationResponse<ProductDto>(productSpecParams.pageSize, productSpecParams.pageIndex, count, mapping);
        }

        public async Task<IEnumerable<ProductTypeDto>> GetAllTypesAsync()
         => mapper.Map<IEnumerable<ProductTypeDto>>(await unitofwork.Repositories<ProductType, int>().GetAllAsync());


        public async Task<ProductDto> GetProductById(int id)
       => mapper.Map<ProductDto>(await unitofwork.Repositories<Product, int>().GetByIdAsync(id));

    }
}
