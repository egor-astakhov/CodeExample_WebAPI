using System;

namespace TitanApi.Core.Models
{
    public class DeviceSession : IPersistent
    {
        public string Id { get; set; }

        public string DeviceId { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime? EndedAt { get; set; }
    }
}
