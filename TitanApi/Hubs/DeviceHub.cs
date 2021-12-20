using Microsoft.AspNetCore.SignalR;

namespace TitanApi.Hubs
{
    public class DeviceHub : Hub
    {
        public const string StatusChanged = nameof(StatusChanged);

        public DeviceHub()
        {
            
        }
    }
}
