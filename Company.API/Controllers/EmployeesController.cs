using AutoMapper;
using Company.API.Errors;
using Company.Core.DTOs;
using Company.Core.Entities;
using Company.Core.Helpers;
using Company.Core.Interfaces;
using Company.Core.Interfaces.Services;
using Company.Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Company.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public EmployeesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _serviceManager.EmployeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound(new ApiResponse(404));

            return Ok(employee);
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<EmployeeDto>>> GetEmployees(
    [FromQuery] EmployeeSpecParams specParams)
        {
            var result = await _serviceManager.EmployeeService.GetEmployeesAsync(specParams);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateEmployee(CreateEmployeeDto dto)
        {
            return Ok();
        }
    }
}