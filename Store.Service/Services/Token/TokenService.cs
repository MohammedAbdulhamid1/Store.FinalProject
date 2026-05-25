using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.IdentityModel.Tokens;
using Store.Core.Entites.Identity;
using Store.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManager)
        {
            var Securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var Credentials = new SigningCredentials(Securitykey, SecurityAlgorithms.HmacSha256Signature);
            var authclaims = new List<Claim>()
{
    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
    new Claim(ClaimTypes.GivenName, user.UserName ?? string.Empty)
};

            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                authclaims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
            }
            var roles = await userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                authclaims.Add(new Claim(ClaimTypes.Role, role));
            };
            var token = new JwtSecurityToken(

                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["Jwt:DurationInDays"])),
                claims: authclaims,
                signingCredentials:Credentials




                );
            return new JwtSecurityTokenHandler().WriteToken(token);
            
            
            

        }
    }
}
