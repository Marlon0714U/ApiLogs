// LogsApi/Models/LogDbContext.cs

using Microsoft.EntityFrameworkCore;

namespace LogsApi.Models
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) { }

        // Definimos la tabla de logs en la base de datos
        public DbSet<Log> Logs { get; set; }
    }
}
