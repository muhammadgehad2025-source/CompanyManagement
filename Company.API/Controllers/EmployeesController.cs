using Company.API.Errors;
using Company.Core.DTOs;
using Company.Core.Helpers;
using Company.Core.Interfaces.Services;
using Company.Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public EmployeesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<Pagination<EmployeeDto>>> GetEmployees(
            [FromQuery] EmployeeSpecParams specParams)
        {
            var result = await _serviceManager.EmployeeService.GetEmployeesAsync(specParams);
            return Ok(result);
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _serviceManager.EmployeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
                return NotFound(new ApiResponse(404));

            return Ok(employee);
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateEmployee(CreateEmployeeDto dto)
        {
            return Ok("Employee created (placeholder)");
        }

        // Public test endpoint
        [AllowAnonymous]
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Employees controller is working");
        }
    }
}