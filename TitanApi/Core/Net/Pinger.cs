using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace TitanApi.Core.Net
{
    public class Pinger : IPinger
    {
        private readonly ILogger<Pinger> _logger;

        public Pinger(ILogger<Pinger> logger)
        {
            _logger = logger;
        }

        public async Task<bool> PingAsync(string host)
        {
            using var pinger = new Ping();

            try
            {
                var result = await pinger.SendPingAsync(host, 5000);

                return result.Status == IPStatus.Success;
            }
            catch (PingException e)
            {
                _logger.LogError(e, $"Cannot ping {host}");
                return false;
            }
        }
    }
}
