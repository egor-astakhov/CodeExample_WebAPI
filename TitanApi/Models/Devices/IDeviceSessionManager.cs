using System.Threading.Tasks;
using TitanApi.Core.Models;

namespace TitanApi.Models.Devices
{
    public interface IDeviceSessionManager
    {
        Task Update(Device device);
    }
}