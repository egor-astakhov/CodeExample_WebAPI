using System;
using System.Collections.Generic;
using System.Linq;
using TitanApi.Core.Models;

namespace TitanApi.Models.Devices
{
    public class DeviceStatusDC
    {
        public string Id { get; }

        public string Type { get; }

        public string Name { get; }

        public string Url { get; }

        public bool IsOnline => Sessions.Any() && Sessions.First().EndedAt is null;

        public IEnumerable<DeviceStatusSessionDC> Sessions { get; set; }

        public DeviceStatusDC(Device device, IEnumerable<DeviceSession> sessions)
        {
            if (device is null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            if (sessions is null)
            {
                throw new ArgumentNullException(nameof(sessions));
            }

            Id = device.Id;
            Type = device.Type;
            Name = device.Name;
            Url = device.Url;

            Sessions = sessions
                .Select(m => new DeviceStatusSessionDC(m))
                .ToList();
        }
    }
}
