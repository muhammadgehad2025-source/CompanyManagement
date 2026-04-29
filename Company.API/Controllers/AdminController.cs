using Company.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AdminController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole(string email, string role)
        {
            var result = await _authService.AddUserToRoleAsync(email, role);
            return Ok(result);
        }
    }
}