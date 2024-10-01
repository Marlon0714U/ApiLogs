// LogsApi/Controllers/LogsController.cs

using Microsoft.AspNetCore.Mvc;
using LogsApi.Models;
using LogsApi.Services;
using System.Threading.Tasks;

namespace LogsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        // POST /api/logs - Crear un nuevo log
        [HttpPost]
        public async Task<IActionResult> CreateLog([FromBody] Log log)
        {
            await _logService.CreateLogAsync(log);
            return CreatedAtAction(nameof(GetLogById), new { id = log.Id }, log);
        }

        // GET /api/logs/{id} - Obtener un log por su ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogById(int id)
        {
            var log = await _logService.GetLogByIdAsync(id);
            if (log == null)
            {
                return NotFound();
            }
            return Ok(log);
        }
    }
}
