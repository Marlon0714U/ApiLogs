// LogsApi/Services/ILogService.cs

using LogsApi.Models;
using System.Threading.Tasks;

namespace LogsApi.Services
{
    public interface ILogService
    {
        Task CreateLogAsync(Log log);
        Task<Log?> GetLogByIdAsync(int id);
        Task<IEnumerable<Log>> GetLogsAsync(string? application, string? logType, DateTime? startDate, DateTime? endDate, int page, int pageSize);
    }
}
