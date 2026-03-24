using Store.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Repositories.Contract
{
    public interface IBasketRepository
    {
        public Task<CustomerBasket?> GetBasketAsync(string basketid);
        public Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket customerBasket);
        public Task<bool> DeleteBasketAsync(string basketid);
    }
}
