using System.Reflection;
using UAS.Business;
using UAS.Data;
using UAS.Database;
using UAS.Dependancies.Business;
using UAS.Dependancies.Data;
using UAS.Dependancies.Database;

namespace api_attendance_system.Services
{
     public class RegisterServices {
        private RegisterServices registerServices;

        public RegisterServices(RegisterServices registerServices)
        {
            this.registerServices = registerServices;
        }

        public IServiceCollection RegisterAllServices(IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IMySQLHelper, MySQLHelper>();

            
            services.AddTransient<IUsers, Users>();
            services.AddTransient<IdUsers, dUsers>();

            return services;
        }
    }
}
