namespace Company.Core.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> CreateToken(string email, string userId);
    }
}