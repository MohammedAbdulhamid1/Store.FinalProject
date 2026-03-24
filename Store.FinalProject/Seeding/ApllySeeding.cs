using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entites.Context;
using Store.Core.Entites.Identity;
using Store.Repository.Identity;
using Store.Repository.Identity.Context;
using Store.Repository.Seeding;

namespace Store.FinalProject.Seeding
{
   
        public class ApllySeeding
        {
            public static async Task ApllySeedingAsync(WebApplication app)
            {
                using (var scope = app.Services.CreateScope())
                {
                    var service = scope.ServiceProvider;
                    var loogerFactory = service.GetRequiredService<ILoggerFactory>();

                    try
                    {
                    var context = service.GetRequiredService<StoreDbContext>();
                    var Identitycontext = service.GetRequiredService<StoreIdentityDbContext>();
                    var UserManger = service.GetRequiredService<UserManager<AppUser>>();
                    
                    await context.Database.MigrateAsync();
                    await Identitycontext.Database.MigrateAsync();
                    await StoreContextSeed.seedAsync(context, loogerFactory);
                    await StoreIdentityContextSeed.SeedAppUserAsync(UserManger);
                    
                }
                    catch (Exception ex)
                    {
                        var looger = loogerFactory.CreateLogger<ApllySeeding>();
                        looger.LogError(ex.Message);
                    }
                }

            }
        }
    }

