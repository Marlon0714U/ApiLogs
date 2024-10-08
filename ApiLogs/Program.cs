// LogsApi/Program.cs

using Microsoft.EntityFrameworkCore;
using LogsApi.Models;
using LogsApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuramos la base de datos SQLite
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("LogsDb")));

// Agregamos el servicio para manejar los logs
builder.Services.AddScoped<ILogService, LogService>();

// Agregamos soporte para controladores (rutas y acciones HTTP)
builder.Services.AddControllers();

var app = builder.Build();

// Aplicar las migraciones automáticamente al iniciar la aplicación
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LogDbContext>();
    db.Database.Migrate(); // Aplica las migraciones pendientes
}

// Si estamos en desarrollo, usamos la página de errores
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

// Habilitamos las rutas de los controladores directamente
app.MapControllers();

// Iniciamos la aplicación
app.Run();
