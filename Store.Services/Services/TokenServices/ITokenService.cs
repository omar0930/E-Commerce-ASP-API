using Store.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Services.TokenServices
{
    public interface ITokenService
    {
        public string GenerateToken(AppUser user);
    }
}
