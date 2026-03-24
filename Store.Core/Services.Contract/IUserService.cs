using Store.Core.Entites.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services.Contract
{
    public interface IUserService
    {
        public Task<UserDto> LoginAsync(LoginDto loginDto);
        public Task<UserDto> RegisterAsync(RegisterDto registerDto);
        public Task<bool> CheckEmailExistAsync(string email);

    }
}
