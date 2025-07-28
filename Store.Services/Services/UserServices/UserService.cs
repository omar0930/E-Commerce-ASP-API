using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.Identity;
using Store.Services.Services.TokenServices;
using Store.Services.Services.UserServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenService tokenService;

        public UserService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ITokenService token)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.tokenService = token;
        }

        public async Task<UserDto> GetCurrentUserDetails(Guid UserId)
        {
            var User = await userManager.FindByIdAsync(UserId.ToString());

            if (User is not null)
            {
                return new UserDto
                {
                    Id = Guid.Parse(User.Id),
                    Email = User.Email,
                    DisplayName = User.displayName,
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDto> Login(LoginDto input)
        {
            var user = await userManager.FindByEmailAsync(input.Email);

            if (user is not null)
            {
                var result = await signInManager.CheckPasswordSignInAsync(user, input.Password, false);
                if (result.Succeeded)
                {
                    return new UserDto
                    {
                        Id = Guid.Parse(user.Id),
                        Email = user.Email,
                        DisplayName = user.displayName,
                        Token = tokenService.GenerateToken(user)
                    };
                }
                else
                {
                    throw new Exception("Login failed");
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDto> Register(RegisterDto input)
        {
            var user = await userManager.FindByEmailAsync(input.Email);

            if (user is not null)
            {
                throw new Exception("User already exists");
            }
            else
            {
                var appUser = new AppUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = input.Email,
                    displayName = input.DisplayName,
                    UserName = input.DisplayName
                };
                var result = await userManager.CreateAsync(appUser, input.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.ToString());
                }
                else
                {
                    return new UserDto
                    {
                        Id = Guid.Parse(appUser.Id),
                        Email = appUser.Email,
                        DisplayName = appUser.displayName,
                        Token = tokenService.GenerateToken(appUser)
                    };
                }
            }
        }
    }
}
