using Microsoft.EntityFrameworkCore;
using SistemasSalgados.Persistence.Configuration.Context;

namespace SistemasSalgados.Bootstrapper
{
    public static class ContextConfig
    {
        public static void ConfigContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<SistemaSalgadosDbContext>(builder =>
            {
                builder.UseSqlServer(connectionString);
            });
        }
    }
}
