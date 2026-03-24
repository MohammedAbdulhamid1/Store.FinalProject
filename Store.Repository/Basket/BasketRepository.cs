using StackExchange.Redis;
using Store.Core.Entites;
using Store.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository.Basket
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string basketid)
        {
           return await _database.KeyDeleteAsync(basketid);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketid)
        {
            var basket = await _database.StringGetAsync(basketid);
            if (basket.IsNullOrEmpty)
            {
                return null;
            }
            else
            {
                return JsonSerializer.Deserialize<CustomerBasket>(basket);
            }
            
            

        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
        {
            var updateorcreate = await _database.StringSetAsync(Basket.Id,
             JsonSerializer.Serialize(Basket),TimeSpan.FromDays(30));
            if (updateorcreate==null) return null;
             return await GetBasketAsync(Basket.Id);

        }
    }
}
