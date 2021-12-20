using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TitanApi.BackgroundServices
{
    public class DeviceWatcherService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<DeviceWatcherService> _logger;

        public DeviceWatcherService(IServiceProvider services, ILogger<DeviceWatcherService> logger)
        {
            _services = services;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Device Watcher service");

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _services.CreateScope();

            try
            {
                await scope.ServiceProvider
                    .GetRequiredService<DeviceWatcher>()
                    .Start(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "DeviceWatcher error");
            }
        }
    }
}
