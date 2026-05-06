using Company.Core.DTOs.Identity;

namespace Company.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);

        // NEW
        Task<UserDto?> GetCurrentUserAsync(string email);
        Task<AddressDto?> GetUserAddressAsync(string email);
        Task<bool> UpdateUserAddressAsync(string email, AddressDto dto);
    }
}