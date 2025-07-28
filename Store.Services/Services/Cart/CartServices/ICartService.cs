using Store.Repository.Cart;
using Store.Services.Services.Cart.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Services.Cart.CartServices
{
    public interface ICartService
    {
        public Task<CartDto> UpdateAsync(CartDto cart);

        public Task<CartDto> GetAsync(Guid id);

        public Task<bool> DeleteAsync(Guid id);
    }
}
