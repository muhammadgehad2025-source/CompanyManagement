using Company.Core.DTOs;
using Company.Core.Helpers;
using Company.Core.Specifications;

namespace Company.Core.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<Pagination<EmployeeDto>> GetEmployeesAsync(EmployeeSpecParams specParams);

        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
    }
}