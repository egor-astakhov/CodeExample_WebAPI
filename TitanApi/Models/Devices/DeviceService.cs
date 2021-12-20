using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TitanApi.Core.Models;
using TitanApi.Hubs;
using TitanApi.Infrastructure;
using TitanApi.Persistence;

namespace TitanApi.Models.Devices
{
    public class DeviceService : IDeviceService
    {
        private const int STATUS_SESSIONS_LIMIT = 10;

        private readonly IDatabase _db;
        private readonly IHubContext<DeviceHub> _deviceHub;
        private readonly IDateTimeProvider _dateTime;

        public DeviceService(
            IDatabase db,
            IHubContext<DeviceHub> deviceHub,
            IDateTimeProvider dateTime)
        {
            _db = db;
            _deviceHub = deviceHub;
            _dateTime = dateTime;
        }

        public async Task<IEnumerable<Device>> GetAsync()
        {
            return await _db.GetListAsync<Device>();
        }

        public async Task<IEnumerable<DeviceDC>> GetDCAsync()
        {
            var devices = await GetAsync();

            return devices.Select(device => new DeviceDC(device));
        }

        public async Task CreateAsync(CreateDeviceDC dc)
        {
            var device = new Device
            {
                Type = dc.Type,
                Name = dc.Name,
                Url = dc.Url
            };

            await _db.InsertAsync(device);
        }

        public async Task<IEnumerable<DeviceStatusDC>> GetStatusDCAsync()
        {
            var devices = await _db.GetListAsync<Device>();

            var tasks = devices.Select(async device =>
            {
                var sessions = await _db.GetQueryable<DeviceSession>()
                    .Where(m => m.DeviceId == device.Id && m.StartedAt > _dateTime.Since)
                    .OrderByDescending(m => m.Id)
                    .Take(STATUS_SESSIONS_LIMIT)
                    .ToListAsync();

                return new DeviceStatusDC(device, sessions);
            });

            var result = (await Task.WhenAll(tasks))
                .OrderBy(m => m.Name)
                .ToList();

            return result;
        }

        public async Task<DeviceSession> GetActiveSessionAsync(Device device, DateTime since)
        {
            var session = await _db.GetCollection<DeviceSession>()
                .Find(m => m.DeviceId == device.Id && m.StartedAt > since && !m.EndedAt.HasValue)
                .Sort(Builders<DeviceSession>.Sort.Descending(m => m.Id))
                .FirstOrDefaultAsync();

            return session;
        }

        public async Task CreateSessionAsync(Device device)
        {
            var session = new DeviceSession
            {
                DeviceId = device.Id,
                StartedAt = DateTime.Now
            };

            await _db.InsertAsync(session);

            await SendStatusChangedSignalAsync();
        }

        public async Task CloseSessionAsync(DeviceSession session)
        {
            session.EndedAt = DateTime.Now;

            await _db.ReplaceAsync(session);

            await SendStatusChangedSignalAsync();
        }

        private async Task SendStatusChangedSignalAsync()
        {
            await _deviceHub.Clients.All.SendAsync(DeviceHub.StatusChanged);
        }
    }
}
