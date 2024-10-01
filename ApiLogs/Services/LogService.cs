
using LogsApi.Models;

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
