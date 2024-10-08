// LogsApi/Controllers/LogsController.cs

using Microsoft.AspNetCore.Mvc;
using LogsApi.Models;
using LogsApi.Services;
using System.Threading.Tasks;

namespace LogsApi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
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

        // LogsApi/Controllers/LogsController.cs

        [HttpGet]
        public async Task<IActionResult> GetLogs(
            [FromQuery] string? application,  // Filtro por nombre de aplicación
            [FromQuery] string? logType,      // Filtro por tipo de log
            [FromQuery] DateTime? startDate,  // Filtro por rango de fecha (inicio)
            [FromQuery] DateTime? endDate,    // Filtro por rango de fecha (fin)
            [FromQuery] int page = 1,         // Paginación: número de página
            [FromQuery] int pageSize = 10     // Paginación: tamaño de página
        )
        {
            var logs = await _logService.GetLogsAsync(application, logType, startDate, endDate, page, pageSize);
            return Ok(logs);
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
