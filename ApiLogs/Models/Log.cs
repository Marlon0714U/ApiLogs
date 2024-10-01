namespace LogsApi.Models
{
    public class Log
    {
        public int Id { get; set; }
        public required string ApplicationName { get; set; }  // Nombre de la aplicación
        public required string LogType { get; set; }  // Tipo de log (info, error, etc.)
        public required string Module { get; set; }  // Módulo o clase que generó el log
        public DateTime Timestamp { get; set; }  // Fecha y hora del log
        public required string Summary { get; set; }  // Resumen breve del log
        public required string Description { get; set; }  // Descripción detallada
    }

}
