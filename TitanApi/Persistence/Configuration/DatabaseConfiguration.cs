using MongoDB.Driver;

namespace TitanApi.Persistence.Configuration
{
    public static class DatabaseConfiguration
    {
        public static void Configure(this IMongoDatabase db)
        {
            var configurations = new ICollectionConfiguration[] 
            { 
                new DeviceConfiguration(db),
                new DeviceSessionConfiguration(db)
            };

            foreach (var config in configurations)
            {
                config.Configure();
            }
        }
    }
}
