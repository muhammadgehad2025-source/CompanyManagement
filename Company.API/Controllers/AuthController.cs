using Company.Core.DTOs.Identity;
using Company.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Company.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AuthController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var user = await _serviceManager.AuthService.GetCurrentUserAsync(email);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterDto dto)
        {
            var result = await _serviceManager.AuthService.RegisterAsync(dto);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto dto)
        {
            var result = await _serviceManager.AuthService.LoginAsync(dto);

            return Ok(result);
        }
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var address = await _serviceManager.AuthService.GetUserAddressAsync(email);

            if (address == null)
                return NotFound();

            return Ok(address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult> UpdateUserAddress(AddressDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var result = await _serviceManager.AuthService.UpdateUserAddressAsync(email, dto);

            if (!result)
                return BadRequest();

            return Ok();
        }
    }
}