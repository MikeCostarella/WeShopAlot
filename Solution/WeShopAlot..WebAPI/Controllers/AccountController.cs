using AutoMapper;
using WeShopAlot.Data.Interfaces;
using WeShopAlot.Data.Models;
using WeShopAlot.Shared.Dtos;
using WeShopAlot.WebAPI.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeShopAlot.Data.Repositories.Interfaces;
using System.Security.Claims;

namespace WeShopAlot.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        #region Member Variables

        private readonly IAppUserRepository appUserRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        #endregion Member Variables

        #region Constructors

        public AccountController(IAppUserRepository appUserRepository, IHttpContextAccessor httpContextAccessor, ITokenService tokenService, IMapper mapper)
        {
            this.appUserRepository = appUserRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.tokenService = tokenService;
        }

        #endregion Constructors

        #region Actions

        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            string emailAddress;
            AppUser appUser = null;
            var claimsPrincipal = httpContextAccessor.HttpContext.User;
            try
            {
                emailAddress = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
                if (emailAddress == null)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            try
            {
                appUser = await appUserRepository.GetByEmailAddressAsync(emailAddress);
            }
            catch (Exception ex)
            {
                throw;
            }
            return new UserDto
            {
                Email = appUser.EmailAddress,
                Token = tokenService.CreateToken(appUser),
                DisplayName = appUser.DisplayName
            };
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var parameterLoginDto = loginDto ?? throw new ArgumentNullException(nameof(loginDto));
            var appUser = await appUserRepository.GetByEmailAddressAsync(loginDto.Email);
            if (appUser == null) return Unauthorized(new ApiResponse(401));
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, appUser.PasswordHash))
            {
                return Unauthorized(new ApiResponse(401));
            }
            return new UserDto
            {
                Email = appUser.EmailAddress,
                Token = tokenService.CreateToken(appUser),
                DisplayName = appUser.DisplayName
            };
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await appUserRepository.EmailExistsAsync(registerDto.Email))
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                { Errors = new[] { "Email address is in use" } });
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                EmailAddress = registerDto.Email,
                PasswordHash = passwordHash
            };
            var jwt = tokenService.CreateToken(user);
            await appUserRepository.InsertAsync(user);
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = jwt,
                Email = user.EmailAddress
            };
        }

        [AllowAnonymous]
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await appUserRepository.EmailExistsAsync(email);
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var claimsPrincipal = this.User;
            var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var emailAddress = claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value;
            var user = await appUserRepository.GetByEmailAddressAsync(emailAddress);
            return mapper.Map<Address, AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var emailAddress = claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value;
            var user = await appUserRepository.GetByEmailAddressAsync(emailAddress);
            user.Address = mapper.Map<AddressDto, Address>(address);
            await appUserRepository.UpdateAsync(user);
            return Ok(mapper.Map<AddressDto>(user.Address));
        }

        #endregion Actions
    }
}
