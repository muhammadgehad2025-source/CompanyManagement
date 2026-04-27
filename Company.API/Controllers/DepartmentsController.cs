using Company.Core.Entities;
using Company.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Company.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IGenericRepository<Department> _repo;

        public DepartmentsController(IGenericRepository<Department> repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Department>>> GetDepartments()
        {
            var departments = await _repo.GetAllAsync();

            return Ok(departments);
        }
    }
}