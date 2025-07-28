using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Cart.Interfaces
{
    public interface ICartRepository
    {
        public Task<CustomerCart> UpdateAsync(CustomerCart cart);

        public Task<CustomerCart> GetAsync(Guid id);

        public Task<bool> DeleteAsync(Guid id);
    }
}
