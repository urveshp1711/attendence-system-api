using api_attendance_system.Handlers;
using UAS.Business;
using UAS.Data;
using UAS.Database;
using UAS.Dependancies.Business;
using UAS.Dependancies.Data;
using UAS.Dependancies.Database;
//using Microsoft.Extensions.DependencyInjection.ConfigSample.Options;

namespace api_attendance_system.Services
{
    public static class CustomServices
    {
        public static IServiceCollection AddDependanciesSingletone(this IServiceCollection services)
        {
            services.AddSingleton<IJwtHandler, JwtHandler>();
            return services;
        }

        public static IServiceCollection AddDependanciesScoped(this IServiceCollection services)
        {
            #region User Controller
            services.AddScoped<IMySQLHelper, MySQLHelper>();
            #endregion User Controller
            return services;
        }

        public static IServiceCollection AddDependanciesTransient(this IServiceCollection services)
        {
            #region User Controller
            services.AddTransient<IUsers, Users>();
            services.AddTransient<IdUsers, dUsers>();
            #endregion User Controller

            return services;
        }
    }
}
