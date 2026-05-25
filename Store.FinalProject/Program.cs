
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Store.Core.Entites;
using Store.Core.Entites.Context;
using Store.Core.Entites.Identity;
using Store.Core.Mapping.Auth;
using Store.Core.Mapping.Basket;
using Store.Core.Mapping.Orders;
using Store.Core.Mapping.Products;
using Store.Core.Repositories.Contract;
using Store.Core.Services.Contract;
using Store.FinalProject.MiddleWares;
using Store.FinalProject.Seeding;
using Store.Repository.Basket;
using Store.Repository.Identity.Context;
using Store.Repository.UnirOfWork;
using Store.Service.Orders;
using Store.Service.Services;
using Store.Service.Services.Token;
using Store.Service.Services.Users;
using System.Text;
using Umbraco.Core.Composing.CompositionExtensions;

namespace Store.FinalProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreDbContext>(
                options =>
                {
                    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Store.Repository"));
                });
            /*identity*/
            builder.Services.AddDbContext<StoreIdentityDbContext>(
              options =>
              {
              options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityConnection"),
                  b => b.MigrationsAssembly("Store.Repository"));
              });
            // =========================================================
            // CORS — allow any localhost origin (dev: Live Server, React, etc.)
            // =========================================================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy
 .WithOrigins(
    "https://moonlit-gaufre-8077a9.netlify.app",
    "https://mohammedstore1.netlify.app",
    "https://willowy-mooncake-cbb891.netlify.app" ,
     "https://mohammedabdulhamidstore.netlify.app"// ? ??? ????? ??
)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            // =========================================================
            // CORS — allow React frontend (localhost:3000)
            // =========================================================
            /*  builder.Services.AddCors(options =>
              {
                  options.AddPolicy("FrontendPolicy", policy =>
                  {
                      policy
                          .WithOrigins(
                              "http://localhost:3000",
                              "https://localhost:3000"
                          )
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                  });
              });*/


            builder.Services.AddScoped<IUnitofwork, UnitOfWork>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddAutoMapper(m=>m.AddProfile(new ProductProfile(builder.Configuration )));
            builder.Services.AddAutoMapper(m => m.AddProfile(new BasketProfile()));
            builder.Services.AddAutoMapper(m => m.AddProfile(new AuthProfile()));
            builder.Services.AddAutoMapper(m => m.AddProfile(new OrderProfile(builder.Configuration)));

            /*  builder.Services.AddIdentity<AppUser, IdentityRole>()
                  .AddEntityFrameworkStores<StoreIdentityDbContext>();*/
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                // ????? ????? ???? ???? (???????)
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
           .AddEntityFrameworkStores<StoreIdentityDbContext>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            /* builder.Services.AddSingleton<IConnectionMultiplexer>(s=>
                 {
                     var connection = builder.Configuration.GetConnectionString("Redis");
                     return ConnectionMultiplexer.Connect(connection);

             });*/
            builder.Services.AddSingleton<IConnectionMultiplexer>(s =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer =builder.Configuration["Jwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                    )
                };
            });

            var app = builder.Build();
            app.UseMiddleware<ExeptionMiddleware>();

            // Configure the HTTP request pipeline.
           /* if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }*/
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseStaticFiles();
          //  app.UseHttpsRedirection();
            // =========================================================
            // CORS must come BEFORE Authentication & Authorization
            // =========================================================
            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();
            
            await ApllySeeding.ApllySeedingAsync(app);
            app.MapControllers();

            app.Run();
        }
    }
}
