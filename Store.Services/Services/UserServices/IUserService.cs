using Store.Data.Entities.Identity;
using Store.Services.Services.UserServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Services.UserServices
{
    public interface IUserService
    {
        Task<UserDto> Login(LoginDto input);

        Task<UserDto> Register(RegisterDto input);
        Task<UserDto> GetCurrentUserDetails(Guid UserId);

    }
}
