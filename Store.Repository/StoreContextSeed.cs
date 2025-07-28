using Microsoft.Extensions.Logging;
using Store.Data.Contexts;
using Store.Data.Entities;
using System.Text.Json;

namespace Store.Repository
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDBContext context, ILoggerFactory Logger)
        {
            try
            {
                if (context.ProductBrands != null && !context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Store.Repository/SeedData/brands.json");
                    var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    if (Brands is not null)
                    {
                        await context.ProductBrands.AddRangeAsync(Brands);
                    }
                }
                if (context.ProductCategories != null && !context.ProductCategories.Any())
                {
                    var categoriesData = File.ReadAllText("../Store.Repository/SeedData/types.json");
                    var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);
                    if (Categories is not null)
                    {
                        await context.ProductCategories.AddRangeAsync(Categories);
                    }
                }
                if (context.Products != null && !context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Store.Repository/SeedData/products.json");
                    var Products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    if (Products is not null)
                    {
                        await context.Products.AddRangeAsync(Products);
                    }
                }
                if (context.DeliveryMethods != null && !context.DeliveryMethods.Any())
                {
                    var deliveryData = File.ReadAllText("../Store.Repository/SeedData/delivery.json");
                    var Deliveries = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                    if (Deliveries is not null)
                    {
                        await context.DeliveryMethods.AddRangeAsync(Deliveries);
                    }
                }
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var logger = Logger.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
