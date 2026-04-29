using Company.Core.DTOs.Identity;
using Company.Core.Interfaces.Services;
using Company.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Company.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var user = new AppUser
            {
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                return errors;
            }

            return await _tokenService.CreateToken(dto.Email, user.Id);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return "Invalid email";

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded)
                return "Invalid password";

            return await _tokenService.CreateToken(dto.Email, user.Id);
        }

        public async Task<string> AddUserToRoleAsync(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return "User not found";

            // FIX: use RoleManager instead
            if (!await _roleManager.RoleExistsAsync(role))
                return "Role does not exist";

            var result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
                return "Failed to assign role";

            return $"User added to {role}";
        }
    }
}