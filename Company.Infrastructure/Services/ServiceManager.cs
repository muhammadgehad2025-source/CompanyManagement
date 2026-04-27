using AutoMapper;
using Company.Core.Interfaces;
using Company.Core.Interfaces.Services;

namespace Company.Infrastructure.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IEmployeeService> _employeeService;

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _employeeService = new Lazy<IEmployeeService>(
                () => new EmployeeService(unitOfWork, mapper));
        }

        public IEmployeeService EmployeeService => _employeeService.Value;
    }
}