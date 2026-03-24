using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specification
{
    public class ProductSpecParams
    {
        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }

        public int pageSize { get; set; } = 5;
        public int pageIndex { get; set; } = 1;
        public string? search { get; set; }
        public string? Search
        {
            get => search;
            set => search = value?.Trim().ToLower();
        }


    }
}
