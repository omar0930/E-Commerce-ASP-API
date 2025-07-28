
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Store.Data.Contexts;
using Store.Data.Entities.Identity;
using Store.Repository;
using Store.Repository.Interfaces;
using Store.Services.Handle_Responses;
using Store.Services.Services;
using Store.Services.Services.TokenServices;
using Store.Web.Extensions;
using Store.Web.Middlewares;

namespace Store.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddDbContext<StoreDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<StoreIdentityDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerDocumentation();
            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));
            });

            var app = builder.Build();

            using (var Service = app.Services.CreateScope())
            {
                var LoggerFactory = Service.ServiceProvider.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = Service.ServiceProvider.GetRequiredService<StoreDBContext>();
                    var context2 = Service.ServiceProvider.GetRequiredService<StoreIdentityDBContext>();

                    var userManager = Service.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                    await context.Database.MigrateAsync();
                    await context2.Database.MigrateAsync();

                    await StoreContextSeed.SeedAsync(context, LoggerFactory);
                    await StoreIdentityContextSeed.SeedAsync(userManager, LoggerFactory);
                }
                catch (Exception ex)
                {
                    var logger = LoggerFactory.CreateLogger<StoreContextSeed>();
                    logger.LogError(ex.Message);
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
