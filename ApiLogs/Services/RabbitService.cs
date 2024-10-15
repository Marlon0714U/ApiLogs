using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;
using LogsApi.Services;

public class RabbitMQConsumer
{
    private readonly string _hostName = "rabbitmq";
    private readonly string _queueName = "logs_queue";
    private readonly string _userName = "user";
    private readonly string _password = "password";
    private IConnection? _connection;
    private IModel? _channel;

    private readonly ILogService? _logService;

    public RabbitMQConsumer(ILogService logService)
    {
        _logService = logService;
    }

    public void StartListening(CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine("Intentando conectar a RabbitMQ...");

            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };

            _connection = factory.CreateConnection();
            Console.WriteLine("Conexión a RabbitMQ establecida.");

            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                                    queue: _queueName,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null
                                    );

            Console.WriteLine($"Conectado a la cola '{_queueName}'.");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine("Mensaje recibido.");

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Mensaje en formato JSON: {message}");

                // Parsear el mensaje (log) y guardarlo en el sistema
                var logData = JsonConvert.DeserializeObject<LogData>(message);
                if (logData != null)
                {
                    ProcessLog(logData);
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    Console.WriteLine("Log Procesado");
                }
                else
                {
                    Console.WriteLine("Error: El mensaje recibido no pudo ser deserializado correctamente.");
                }
            };

            _channel.BasicConsume(
                                    queue: _queueName,
                                    autoAck: false,
                                    consumer: consumer
                                );

            Console.WriteLine(" [*] Waiting for logs.");

            // Ciclo continuo para mantener el consumidor escuchando
            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(100);  // Pequeña pausa para evitar consumir CPU innecesariamente
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en StartListening: {ex.Message}");
        }
        finally
        {
            Dispose();
        }
    }

    private void ProcessLog(LogData log)
    {
        Console.WriteLine($"Recibido log - Tipo: {log.LogType}, Módulo: {log.Module}, Resumen: {log.Summary}");
        try
        {
            var logModel = new LogsApi.Models.Log
            {
                ApplicationName = log.ApplicationName,
                LogType = log.LogType,
                Module = log.Module,
                Summary = log.Summary,
                Description = log.Description,
                Timestamp = log.Timestamp
            };
            _logService?.CreateLogAsync(logModel);
            Console.WriteLine("Log guardado correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar el log: {ex.Message}");
        }
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}

// Define un modelo para los datos del log
public class LogData
{
    public required string ApplicationName { get; set; }
    public required string LogType { get; set; }
    public required string Module { get; set; }
    public required string Summary { get; set; }
    public required string Description { get; set; }
    public DateTime Timestamp { get; set; }
}
