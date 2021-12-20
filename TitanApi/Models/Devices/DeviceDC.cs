using TitanApi.Core.Models;

namespace TitanApi.Models.Devices
{
    public class DeviceDC
    {
        public string Id { get; }

        public string Type { get; }

        public string Name { get; }

        public string Url { get; }

        public DeviceDC(Device device)
        {
            Id = device.Id;
            Type = device.Type;
            Name = device.Name;
            Url = device.Url;
        }
    }
}
