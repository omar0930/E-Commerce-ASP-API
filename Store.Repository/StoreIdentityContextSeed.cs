using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class StoreIdentityContextSeed
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, ILoggerFactory loggerFactory)
        {
            if (!userManager.Users.Any())
            {
                try
                {
                    var user = new AppUser
                    {
                        UserName = "admin",
                        displayName = "admin",
                        Email = "admin@gmail.com",
                        EmailConfirmed = true,
                        address = new Address
                        {
                            FirstName = "admin",
                            LastName = "admin",
                            Street = "Mohamed abd al hameed",
                            City = "Cairo",
                            State = "Giza",
                            ZipCode = "123456",
                        }
                    };
                    await userManager.CreateAsync(user, "Password123!");
                }
                catch (Exception ex)
                {
                    loggerFactory.CreateLogger<StoreIdentityContextSeed>().LogError(ex, "An error occurred seeding the DB.");
                }
            }
        }
    }
}
