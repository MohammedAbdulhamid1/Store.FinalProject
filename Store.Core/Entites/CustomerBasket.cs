using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entites
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public List<Basketitem> Items { get; set; } = new List<Basketitem>();
        public int? DeliveryNethod { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
