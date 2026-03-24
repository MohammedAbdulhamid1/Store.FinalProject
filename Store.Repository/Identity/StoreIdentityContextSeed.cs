using Microsoft.AspNetCore.Identity;
using Store.Core.Entites.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Identity
{
    public static class StoreIdentityContextSeed
    {
        public async static Task SeedAppUserAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    Email = "test256t@gmail.com",
                    DisplayName = "Mohammedabdo",
                    UserName = "mohammed.abdo",
                    PhoneNumber = "01221555555",
                    Address = new Address()
                    {
                        FName="Mohammed",
                        LName="Abdo",
                        City="Banha",
                        Country="Egypt",
                        Street="Elsab"
                    }

                };
                await _userManager.CreateAsync(user,"P@ssW0rd");
            }
        }
        
    }
}
