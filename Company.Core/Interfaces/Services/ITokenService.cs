namespace Company.Core.Interfaces.Services
{
    public interface ITokenService
    {
        string CreateToken(string email, string userId);
    }
}