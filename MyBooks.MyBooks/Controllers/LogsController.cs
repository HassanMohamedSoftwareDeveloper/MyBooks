using Microsoft.AspNetCore.Mvc;
using MyBooks.MyBooks.Data.Services;

namespace MyBooks.MyBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly LogsService logsService;

        public LogsController(LogsService logsService)
        {
            this.logsService = logsService;
        }

        [HttpGet("get-all-logs")]
        public IActionResult GetAllLogs()
        {
            var result = logsService.GetAllLogs();
            return Ok(result);
        }
    }
}
