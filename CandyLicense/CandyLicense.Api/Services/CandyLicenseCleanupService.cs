using CandyLicense.Api.Application.Commands;
using MediatR;

namespace CandyLicense.Api.Services;

public class CandyLicenseCleanupService : IHostedService, IDisposable
{
    private readonly ILogger<CandyLicenseCleanupService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private Timer? _timer = null;

    public CandyLicenseCleanupService(ILogger<CandyLicenseCleanupService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Cleanup service started");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(1));

        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        using var scope = _serviceProvider.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new FreeExpiredLicenses.Command());
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Cleanup service is stopping");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}