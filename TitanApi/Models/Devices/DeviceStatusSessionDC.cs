using System;
using TitanApi.Core.Models;

namespace TitanApi.Models.Devices
{
    public class DeviceStatusSessionDC
    {
        public string Id { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime? EndedAt { get; set; }

        public DeviceStatusSessionDC(DeviceSession session)
        {
            Id = session.Id;
            StartedAt = session.StartedAt;
            EndedAt = session.EndedAt;
        }
    }
}
