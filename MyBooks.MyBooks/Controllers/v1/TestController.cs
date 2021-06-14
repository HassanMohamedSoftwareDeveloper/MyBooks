using Microsoft.AspNetCore.Mvc;

namespace MyBooks.MyBooks.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [ApiVersion("1.2")]
    [ApiVersion("1.3")]
    [Route("api/[controller]")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("get-test-data"),MapToApiVersion("1.0")]
        public IActionResult Get()
        {
            return Ok("This is TestController v1.0");
        }
        [HttpGet("get-test-data"), MapToApiVersion("1.1")]
        public IActionResult Get11()
        {
            return Ok("This is TestController v1.1");
        }
        [HttpGet("get-test-data"), MapToApiVersion("1.2")]
        public IActionResult Get12()
        {
            return Ok("This is TestController v1.2");
        }
        [HttpGet("get-test-data"), MapToApiVersion("1.3")]
        public IActionResult Get13()
        {
            return Ok("This is TestController v1.3");
        }
    }
}
