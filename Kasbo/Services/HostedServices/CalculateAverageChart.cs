using Kasbo.Models;
using Kasbo.Services;

namespace Kasbo.Services.HostedServices;

public class CalculateAverageChart : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly ILogger<CalculateAverageChart> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public CalculateAverageChart(IServiceScopeFactory serviceScopeFactory, ILogger<CalculateAverageChart> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));

        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        try
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var chartService = scope.ServiceProvider.GetRequiredService<IChartService>();

                await chartService.CalculateAvarage();
            }
            _logger.Log(LogLevel.Information, "CalculateAvarage Is successful");
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, "CalculateAvarage Is Error");

            throw new Exception(ex.Message);
        }


    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
