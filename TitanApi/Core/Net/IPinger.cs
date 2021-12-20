using System.Threading.Tasks;

namespace TitanApi.Core.Net
{
    public interface IPinger
    {
        Task<bool> PingAsync(string host);
    }
}