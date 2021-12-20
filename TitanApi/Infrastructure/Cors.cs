using Microsoft.Extensions.DependencyInjection;

namespace TitanApi.Infrastructure
{
    public static class Cors
    {
        //public const string HubPolicy = nameof(HubPolicy);

        public static void SetupCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder
                    .WithOrigins("http://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());

                //options.AddPolicy(HubPolicy, builder => builder
                //    .WithOrigins("http://localhost:8080")
                //    .AllowAnyHeader()
                //    .WithMethods("GET", "POST")
                //    .AllowCredentials());
            });
        }
    }
}
