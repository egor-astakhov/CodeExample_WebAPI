using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TitanApi.Core.Models;

namespace TitanApi.Models.Devices
{
    public interface IDeviceService
    {
        Task<IEnumerable<Device>> GetAsync();

        Task<IEnumerable<DeviceDC>> GetDCAsync();

        Task CreateAsync(CreateDeviceDC dc);

        Task<IEnumerable<DeviceStatusDC>> GetStatusDCAsync();

        Task<DeviceSession> GetActiveSessionAsync(Device device, DateTime since);

        Task CreateSessionAsync(Device device);

        Task CloseSessionAsync(DeviceSession session);
    }
}
