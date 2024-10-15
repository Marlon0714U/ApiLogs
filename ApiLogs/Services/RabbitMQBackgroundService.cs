using LogsApi.Services;

public class RabbitMQBackgroundService : BackgroundService
{
    private readonly RabbitMQConsumer _consumer;

    private readonly ILogService _logService;

    public RabbitMQBackgroundService(ILogService logService)
    {
        _logService = logService;
        _consumer = new RabbitMQConsumer(_logService);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(() => _consumer.StartListening(stoppingToken), stoppingToken);
    }

    public override void Dispose()
    {
        _consumer.Dispose();
        base.Dispose();
    }
}
