using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Entites.Dtos.Auth;
using Store.Core.Entites.Identity;
using Store.Core.Services.Contract;
using Store.FinalProject.Errors;
using Store.FinalProject.Extenstions;
using System.Security.Claims;

namespace Store.FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService,UserManager<AppUser> userManager,ITokenService tokenService,IMapper mapper)
        {
            _userService = userService;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<LoginDto>> Login(LoginDto loginDto)
        {
            var user = await _userService.LoginAsync(loginDto);
            if (user == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            return Ok(user);

        }
        [HttpPost("Register")]
        public async Task<ActionResult<LoginDto>> Register(RegisterDto registerDto)
        {
            var user = await _userService.RegisterAsync(registerDto);
            if (user == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest,"invalid registration"));
            return Ok(user);

        }
       

        [HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var UserEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(UserEmail);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
            
        }
        
        [HttpGet("Address")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUserAddress()
        {
            var user = await _userManager.FindByEmailWithAddressAsync(User);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<AddressDto>(user.Address));

        }
    }
}
