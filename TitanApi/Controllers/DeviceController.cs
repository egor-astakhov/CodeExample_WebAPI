using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TitanApi.Models.Devices;

namespace TitanApi.Controllers
{
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        /// <summary>
        /// Get Devices
        /// </summary>
        /// <returns>Array of <see cref="DeviceDC"/></returns>
        [HttpGet]
        [Route("api/devices")]
        public async Task<IEnumerable<DeviceDC>> GetDevicesAsync()
        {
            return await _deviceService.GetDCAsync();
        }

        /// <summary>
        /// Create Device
        /// </summary>
        /// <param name="dc"><see cref="CreateDeviceDC"/></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/devices")]
        public async Task<ActionResult> CreateDeviceAsync([FromBody]CreateDeviceDC dc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _deviceService.CreateAsync(dc);

            return Ok(); //Should return Created to follow REST approach
        }

        /// <summary>
        /// Get Devices status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/devices/status")]
        public async Task<IEnumerable<DeviceStatusDC>> GetDevicesStatusAsync()
        {
            return await _deviceService.GetStatusDCAsync();
        }
    }
}
