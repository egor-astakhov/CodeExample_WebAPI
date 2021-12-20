using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TitanApi.Models.Devices;
using TitanApi.Persistence.Configuration;
using TitanApi.Persistence;
using TitanApi.Infrastructure;
using TitanApi.BackgroundServices;
using TitanApi.Core.Net;

namespace TitanApi
{
    internal static class StartupExtensions
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("TitanDb");

            var settings = MongoClientSettings.FromConnectionString(connectionString);

            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            services.AddSingleton(provider => new MongoClient(settings));

            services.AddSingleton(provider =>
            {
                var db = provider
                    .GetRequiredService<MongoClient>()
                    .GetDatabase(new MongoUrlBuilder(connectionString).DatabaseName);

                db.Configure();

                return db;
            });

            services.AddScoped<IDatabase, Database>();
        }
        
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IDeviceService, DeviceService>();
        }

        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.SetupCors();
        }

        public static void AddDeviceWatcher(this IServiceCollection services)
        {
            services.AddHostedService<DeviceWatcherService>();
            services.AddTransient<DeviceWatcher>();
            services.AddTransient<IDeviceSessionManager, DeviceSessionManager>();
            services.AddSingleton<IPinger, Pinger>();
        }
    }
}
