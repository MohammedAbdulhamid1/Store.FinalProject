using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entites.Identity;
using System.Security.Claims;
using Umbraco.Core.Models.Membership;

namespace Store.FinalProject.Extenstions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByEmailWithAddressAsync(this UserManager<AppUser> userManager,ClaimsPrincipal User)
        {
            var UserEmail = User.FindFirstValue(ClaimTypes.Email);
            if (UserEmail is null) return null;
            var user = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u=>u.Email== UserEmail);
            return user;

        }
    }
}
