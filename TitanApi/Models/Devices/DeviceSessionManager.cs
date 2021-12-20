using System.Threading.Tasks;
using TitanApi.Core.Models;
using TitanApi.Core.Net;
using TitanApi.Infrastructure;

namespace TitanApi.Models.Devices
{
    public class DeviceSessionManager : IDeviceSessionManager
    {
        private readonly IDeviceService _deviceService;
        private readonly IPinger _pinger;
        private readonly IDateTimeProvider _dateTime;

        public DeviceSessionManager(
            IDeviceService deviceService,
            IPinger pinger,
            IDateTimeProvider dateTime
            )
        {
            _deviceService = deviceService;
            _pinger = pinger;
            _dateTime = dateTime;
        }

        public async Task Update(Device device)
        {
            var activeSession = await _deviceService.GetActiveSessionAsync(device, _dateTime.Since);
            var isAvailable = await _pinger.PingAsync(device.Url);

            if (activeSession is null && !isAvailable)
            {
                //No active session and Device is offline --> nothing to do
                return;
            }

            if (activeSession is null && isAvailable)
            {
                //Device turned on --> create session
                await _deviceService.CreateSessionAsync(device);
                return;
            }

            if (activeSession is not null && isAvailable)
            {
                //Active session exists and Device is online --> nothing to do
                return;
            }

            if (activeSession is not null && !isAvailable)
            {
                //Active session exists but Device is turned off --> close the session
                await _deviceService.CloseSessionAsync(activeSession);
                return;
            }
        }
    }
}
