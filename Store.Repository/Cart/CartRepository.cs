using StackExchange.Redis;
using Store.Repository.Cart.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository.Cart
{
    public class CartRepository : ICartRepository
    {
        private readonly IDatabase _database;
        public CartRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _database.KeyDeleteAsync(id.ToString());

            return true;
        }

        public async Task<CustomerCart> GetAsync(Guid id)
        {
            var cart = await _database.StringGetAsync(id.ToString());

            return (cart.IsNullOrEmpty) ? null : JsonSerializer.Deserialize<CustomerCart>(cart);
        }

        public async Task<CustomerCart> UpdateAsync(CustomerCart cart)
        {
            var status = await _database.StringSetAsync(cart.id.ToString(), JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
            return status ? cart : null;
        }
    }
}
