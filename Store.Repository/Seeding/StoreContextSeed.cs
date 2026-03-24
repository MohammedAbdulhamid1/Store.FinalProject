using Microsoft.Extensions.Logging;
using Store.Core.Entites;
using Store.Core.Entites.Context;
using Store.Core.Entites.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository.Seeding
{
    public class StoreContextSeed
    {
        public static async Task seedAsync(StoreDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (context.ProductTypes != null && !context.ProductTypes.Any()) //if brands not any in table do this code
                {
                    // to Read Json File
                    var TypesData = File.ReadAllText("../Store.Repository/SeedData/Types.json");
                    // to convert Json file to List
                    var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                    // to add List in dataBase
                    if (Types is not null)
                    {

                        await context.Set<ProductType>().AddRangeAsync(Types);

                    }

                }
                if (context.ProductBrands != null && !context.ProductBrands.Any()) //if brands not any in table do this code
                {
                    // to Read Json File
                    var BrandsData = File.ReadAllText("../Store.Repository/SeedData/brands.json");
                    // to convert Json file to List
                    var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                    // to add List in dataBase
                    if (Brands is not null)
                    {

                        await context.Set<ProductBrand>().AddRangeAsync(Brands);

                    }

                }

                if (context.Products != null && !context.Products.Any()) //if brands not any in table do this code
                {
                    // to Read Json File
                    var ProductData = File.ReadAllText("../Store.Repository/SeedData/products.json");
                    // to convert Json file to List
                    var product = JsonSerializer.Deserialize<List<Product>>(ProductData);
                    // to add List in dataBase
                    if (product is not null)
                    {

                        await context.Set<Product>().AddRangeAsync(product);

                    }

                }
                if (context.DeliveryMethods != null && !context.DeliveryMethods.Any()) //if brands not any in table do this code
                {
                    // to Read Json File
                    var DeliveryData = File.ReadAllText("../Store.Repository/SeedData/delivery.json");
                    // to convert Json file to List
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);
                    // to add List in dataBase
                    if (deliveryMethods is not null)
                    {

                        await context.Set<DeliveryMethod>().AddRangeAsync(deliveryMethods);

                    }

                }

                await context.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                await context.SaveChangesAsync();
                var looger = loggerFactory.CreateLogger<StoreDbContext>();
                looger.LogError(ex.Message);
            }
        }
    }
}
