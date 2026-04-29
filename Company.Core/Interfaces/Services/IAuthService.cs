using Company.Core.DTOs.Identity;

namespace Company.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);

        Task<string> LoginAsync(LoginDto dto);
        Task<string> AddUserToRoleAsync(string email, string role);
    }
}