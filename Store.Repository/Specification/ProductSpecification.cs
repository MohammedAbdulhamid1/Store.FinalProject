using Microsoft.AspNetCore.Mvc;
using Store.Core.Entites;
using Store.Core.Specification;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Store.Repository.Specification
{
    public class ProductSpecification:BaseSpecification<Product, int> 
    {
        public ProductSpecification(int id):base(p=>p.Id==id)
        {
            Include.Add(p => p.Brand);
            Include.Add(p => p.Type);

        }
        public ProductSpecification(ProductSpecParams productSpecParams) : base(
            p =>
             
            (string.IsNullOrEmpty(productSpecParams.Search) || p.Name.ToLower().Contains(productSpecParams.Search))
            &&
            (!productSpecParams.BrandId.HasValue || productSpecParams.BrandId == p.BrandId)
            &&
            (!productSpecParams.TypeId.HasValue || productSpecParams.TypeId == p.TypeId)
           
            )
        {
            Include.Add(p => p.Brand);
            Include.Add(p => p.Type);
            if (!string.IsNullOrEmpty(productSpecParams.Sort))
            {
                switch (productSpecParams.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;



                }

            }
            else
            {
                AddOrderBy(p => p.Name);
            }
            ApplyPagination(productSpecParams.pageSize * (productSpecParams.pageIndex - 1), productSpecParams.pageSize);

        }
    }
}
