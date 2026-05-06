using Company.Core.DTOs.Identity;
using Company.Core.Interfaces.Services;
using Company.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.Infrastructure.Services
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

            return await _tokenService.CreateToken(user.Email, user.Id);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return "Invalid email";

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded)
                return "Invalid password";

            return await _tokenService.CreateToken(user.Email, user.Id);
        }

        public async Task<string> AddUserToRoleAsync(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return "User not found";

            if (!await _roleManager.RoleExistsAsync(role))
                return "Role does not exist";

            var result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
                return "Failed to assign role";

            return $"User added to {role}";
        }

        public async Task<UserDto?> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return null;

            return new UserDto
            {
                Email = user.Email
            };
        }

        public async Task<AddressDto?> GetUserAddressAsync(string email)
        {
            var user = await _userManager.Users
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user?.Address == null)
                return null;

            return new AddressDto
            {
                FirstName = user.Address.FirstName,
                LastName = user.Address.LastName,
                Street = user.Address.Street,
                City = user.Address.City,
                Country = user.Address.Country
            };
        }

        public async Task<bool> UpdateUserAddressAsync(string email, AddressDto dto)
        {
            var user = await _userManager.Users
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null) return false;

            user.Address = new Address
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Street = dto.Street,
                City = dto.City,
                Country = dto.Country
            };

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}