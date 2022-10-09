using Microsoft.EntityFrameworkCore;
using SistemasSalgados.Persistence.Configuration.Context;
using SistemasSalgados.Services.Entities.Auth;
using SistemasSalgados.Services.Interfaces.Auth;

namespace SistemasSalgados.Bootstrapper
{
    public static class IocConfig
    {
        public static void ConfigIoc(this IServiceCollection services)
        {
            Context(services);
            Auth(services);
        }

        public static void Auth(IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRoleService, UserRoleService>();
        }

        private static void Context(IServiceCollection services)
        {
            services.AddTransient<DbContext, SistemaSalgadosDbContext>();
        }
    }
}
