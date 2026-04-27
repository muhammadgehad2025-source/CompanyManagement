using AutoMapper;
using Company.Core.DTOs;
using Company.Core.Entities;
using Company.Core.Helpers;
using Company.Core.Interfaces;
using Company.Core.Interfaces.Services;
using Company.Core.Specifications;

namespace Company.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Pagination<EmployeeDto>> GetEmployeesAsync(EmployeeSpecParams specParams)
        {
            var spec = new EmployeeSpecification(specParams);

            var employees = await _unitOfWork.Repository<Employee>().GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Employee>, IReadOnlyList<EmployeeDto>>(employees);

            var countSpec = new EmployeeCountSpecification(specParams);

            var count = await _unitOfWork.Repository<Employee>().CountAsync(countSpec);

            return new Pagination<EmployeeDto>(
                specParams.PageIndex,
                specParams.PageSize,
                count,
                data
            );
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var spec = new EmployeeByIdSpecification(id);

            var employee = await _unitOfWork.Repository<Employee>().GetEntityWithSpecAsync(spec);

            if (employee == null)
                return null;

            return _mapper.Map<EmployeeDto>(employee);
        }
    }
}