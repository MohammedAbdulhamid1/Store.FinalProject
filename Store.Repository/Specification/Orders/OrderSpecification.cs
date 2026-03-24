using Store.Core.Entites.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.Orders
{
    public class OrderSpecification:BaseSpecification<Order,int>
    {
        public OrderSpecification(string buyerEmail,int orderId):base(o=>o.BuyerEmail==buyerEmail&& o.Id==orderId)
        {
            Include.Add(o => o.DeliveryMethod);
            Include.Add(o => o.Items);
            
        }
        public OrderSpecification(string buyerEmail) : base(o => o.BuyerEmail == buyerEmail)
        {
            Include.Add(o => o.DeliveryMethod);
            Include.Add(o => o.Items);

        }
    }
}
