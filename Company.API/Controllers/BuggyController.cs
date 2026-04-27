using Microsoft.AspNetCore.Mvc;
using Company.API.Errors;

namespace Company.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfound")]
        public ActionResult GetNotFound()
        {
            return NotFound(new ApiResponse(404));
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            throw new Exception("Test server error");
        }
    }
}