using Moq;
using System;
using TitanApi.Core.Models;
using TitanApi.Core.Net;
using TitanApi.Infrastructure;
using TitanApi.Models.Devices;
using Xunit;

namespace TitanApi.Test.Models.Devices
{
    public class DeviceSessionManagerTest
    {
        public class Update
        {
            private Mock<IDeviceService> _deviceServiceMock;
            private Mock<IPinger> _pingerMock;
            private IDeviceSessionManager _deviceSessionManager;

            public Update()
            {
                _deviceServiceMock = new Mock<IDeviceService>();
                _pingerMock = new Mock<IPinger>();
                var dateTimeProvider = new Mock<IDateTimeProvider>();

                _deviceSessionManager = new DeviceSessionManager(
                    _deviceServiceMock.Object,
                    _pingerMock.Object,
                    dateTimeProvider.Object);
            }

            [Fact]
            public void When_NoSession_And_IsNotAvailable_DoNothing()
            {
                _deviceServiceMock
                    .Setup(m => m.GetActiveSessionAsync(It.IsAny<Device>(), It.IsAny<DateTime>()))
                    .ReturnsAsync((DeviceSession)null);

                _pingerMock.Setup(m => m.PingAsync(It.IsAny<string>()))
                    .ReturnsAsync(false);

                _deviceSessionManager.Update(new Device());

                _deviceServiceMock
                    .Verify(m => m.CreateSessionAsync(It.IsAny<Device>()), Times.Never);

                _deviceServiceMock
                    .Verify(m => m.CloseSessionAsync(It.IsAny<DeviceSession>()), Times.Never);
            }

            [Fact]
            public void When_NoSession_And_IsAvailable_CreateSession()
            {
                _deviceServiceMock
                    .Setup(m => m.GetActiveSessionAsync(It.IsAny<Device>(), It.IsAny<DateTime>()))
                    .ReturnsAsync((DeviceSession)null);

                _pingerMock.Setup(m => m.PingAsync(It.IsAny<string>()))
                    .ReturnsAsync(true);

                var device = new Device();

                _deviceSessionManager.Update(device);

                _deviceServiceMock
                    .Verify(m => m.CreateSessionAsync(device), Times.Once);

                _deviceServiceMock
                    .Verify(m => m.CloseSessionAsync(It.IsAny<DeviceSession>()), Times.Never);
            }

            [Fact]
            public void When_SessionExists_And_IsAvailable_DoNothing()
            {
                _deviceServiceMock
                    .Setup(m => m.GetActiveSessionAsync(It.IsAny<Device>(), It.IsAny<DateTime>()))
                    .ReturnsAsync(new DeviceSession());

                _pingerMock.Setup(m => m.PingAsync(It.IsAny<string>()))
                    .ReturnsAsync(true);

                _deviceSessionManager.Update(new Device());

                _deviceServiceMock
                    .Verify(m => m.CreateSessionAsync(It.IsAny<Device>()), Times.Never);

                _deviceServiceMock
                    .Verify(m => m.CloseSessionAsync(It.IsAny<DeviceSession>()), Times.Never);
            }

            [Fact]
            public void When_SessionExists_And_IsNotAvailable_CloseSession()
            {
                var deviceSession = new DeviceSession();

                _deviceServiceMock
                    .Setup(m => m.GetActiveSessionAsync(It.IsAny<Device>(), It.IsAny<DateTime>()))
                    .ReturnsAsync(deviceSession);

                _pingerMock.Setup(m => m.PingAsync(It.IsAny<string>()))
                    .ReturnsAsync(false);

                _deviceSessionManager.Update(new Device());

                _deviceServiceMock
                    .Verify(m => m.CreateSessionAsync(It.IsAny<Device>()), Times.Never);

                _deviceServiceMock
                    .Verify(m => m.CloseSessionAsync(deviceSession), Times.Once);
            }
        }
    }
}
