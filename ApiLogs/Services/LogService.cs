
using LogsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LogsApi.Services
{
    public class LogService : ILogService
    {
        private readonly LogDbContext _context;

        public LogService(LogDbContext context)
        {
            _context = context;
        }

        // Crear un nuevo log en la base de datos
        public async Task CreateLogAsync(Log log)
        {
            log.Timestamp = DateTime.UtcNow;  // Asignar la fecha actual al log
            _context.Logs.Add(log);  // Agregar el log a la base de datos
            await _context.SaveChangesAsync();  // Guardar los cambios
        }

        public async Task<IEnumerable<Log>> GetLogsAsync(
            string? application, string? logType, DateTime? startDate, DateTime? endDate, int page, int pageSize)
        {
            var query = _context.Logs.AsQueryable();

            // Filtro por aplicación
            if (!string.IsNullOrEmpty(application))
            {
                query = query.Where(l => l.ApplicationName == application);
            }

            // Filtro por tipo de log
            if (!string.IsNullOrEmpty(logType))
            {
                query = query.Where(l => l.LogType == logType);
            }

            // Filtro por rango de fechas
            if (startDate.HasValue)
            {
                query = query.Where(l => l.Timestamp >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(l => l.Timestamp <= endDate.Value);
            }

            // Ordenar por fecha de creación
            query = query.OrderBy(l => l.Timestamp);

            // Paginación
            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        // Obtener un log por su ID
        public async Task<Log?> GetLogByIdAsync(int id)  // Retorno opcional Log?
        {
            var log = await _context.Logs.FindAsync(id);

            if (log == null)
            {
                throw new Exception($"No se encontró el log con el ID {id}.");
            }

            return log;
        }
    }
}
