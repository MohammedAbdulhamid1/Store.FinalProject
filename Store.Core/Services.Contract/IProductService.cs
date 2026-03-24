using Microsoft.AspNetCore.Mvc;
using Store.Core.Entites;
using Store.Core.Entites.Dtos;
using Store.Core.Helper;
using Store.Core.Specification;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services.Contract
{
    public interface IProductService
    {
        Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpecParams);
        Task<IEnumerable<ProductBrandDto>>GetAllBrandsAsync();
        Task<IEnumerable<ProductTypeDto>> GetAllTypesAsync();
        Task <ProductDto> GetProductById(int id);


    }
}
