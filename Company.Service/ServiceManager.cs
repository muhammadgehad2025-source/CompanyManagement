using Company.Core.Interfaces.Services;
namespace Company.Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IEmployeeService> _employeeService;
        private readonly Lazy<IAuthService> _authService;

        public ServiceManager(
            Func<IEmployeeService> employeeServiceFactory,
            Func<IAuthService> authServiceFactory)
        {
            _employeeService = new Lazy<IEmployeeService>(employeeServiceFactory);
            _authService = new Lazy<IAuthService>(authServiceFactory);
        }

        public IEmployeeService EmployeeService => _employeeService.Value;

        public IAuthService AuthService => _authService.Value;
    }
}