using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TitanApi.Models.Devices;

namespace TitanApi.BackgroundServices
{
    public class DeviceWatcher 
    {
        private readonly TimeSpan _delay;

        private readonly IDeviceService _deviceService;
        private readonly IDeviceSessionManager _deviceSessionManager;
        private readonly ILogger<DeviceWatcher> _logger;

        public DeviceWatcher(
            IConfiguration configuration,
            IDeviceService deviceService, 
            IDeviceSessionManager deviceSessionManager,
            ILogger<DeviceWatcher> logger)
        {
            _delay = TimeSpan.FromSeconds(configuration.GetValue<int>("DeviceWatcherDelay"));

            _deviceService = deviceService;
            _deviceSessionManager = deviceSessionManager;
            _logger = logger;
        }

        public async Task Start(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"Watch at {DateTime.Now}");

                var devices = await _deviceService.GetAsync();

                var tasks = devices.Select(device => _deviceSessionManager.Update(device));

                await Task.WhenAll(tasks);

                await Task.Delay(_delay, stoppingToken);
            }
        }
    }
}
