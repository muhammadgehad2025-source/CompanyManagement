namespace Company.Core.Interfaces.Services
{
    public interface IServiceManager
    {
        IEmployeeService EmployeeService { get; }

        IAuthService AuthService { get; }
    }
}