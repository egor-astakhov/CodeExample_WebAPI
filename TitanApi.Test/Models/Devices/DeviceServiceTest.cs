using Moq;
using Shouldly;
using System.Threading.Tasks;
using TitanApi.Models.Devices;
using TitanApi.Persistence;
using TitanApi.Core.Models;
using Xunit;

namespace TitanApi.Test.Models.Devices
{
    public class DeviceServiceTest
    {
        public class CreateDevice
        {
            private readonly Mock<IDatabase> _dbMock;
            private readonly DeviceService _deviceService;

            public CreateDevice()
            {
                _dbMock = new Mock<IDatabase>();

                _deviceService = new DeviceService(_dbMock.Object, null, null);
            }

            [Fact]
            public async Task Success()
            {
                var dc = new CreateDeviceDC
                {
                    Name = "qwe",
                    Url = "https://qwe.abc.ru"
                };

                _dbMock
                    .Setup(m => m.InsertAsync(It.IsAny<Device>()))
                    .Callback<Device>(device => 
                    {
                        device.Name.ShouldBe(dc.Name);
                        device.Url.ShouldBe(dc.Url);
                    });

                await _deviceService.CreateAsync(dc);
            }
        }
    }
}
