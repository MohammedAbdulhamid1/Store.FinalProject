using Store.Core.Entites;
using Store.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification
{
    public class ProductWithCountSpecification:BaseSpecification<Product,int>
    {
        public ProductWithCountSpecification(ProductSpecParams productSpecParams) : base(
            p =>
             (string.IsNullOrEmpty(productSpecParams.Search) || p.Name.ToLower().Contains(productSpecParams.Search))
            &&
            (!productSpecParams.BrandId.HasValue || productSpecParams.BrandId == p.BrandId)
            &&
            (!productSpecParams.TypeId.HasValue || productSpecParams.TypeId == p.TypeId)
            )
        {
            

          
           

        }
    }
}
