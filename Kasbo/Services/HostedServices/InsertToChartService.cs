using Kasbo.Models;

namespace Kasbo.Services.HostedServices;

public class InsertToChartService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly ILogger<InsertToChartService> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;
    public InsertToChartService(IServiceScopeFactory serviceScopeFactory, ILogger<InsertToChartService> logger)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

        return Task.CompletedTask;

    }

    private async void DoWork(object state)
    {
        try
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var chartService = scope.ServiceProvider.GetRequiredService<IChartService>();

                AddChartDTO chart = new AddChartDTO
                {
                    Open = GenerateRandom(),
                    Close = GenerateRandom(),
                    High = GenerateRandom(),
                    Low = GenerateRandom()
                };
                await chartService.Add(chart);

                _logger.Log(LogLevel.Information, "Add chart HostedService successful");
            }
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, "Add chart HostedService Error");
            throw new Exception(ex.Message);
        }


    }


    private int GenerateRandom()
    {
        Random random = new Random();
        int number = random.Next(1000, 9999);
        return number;
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
