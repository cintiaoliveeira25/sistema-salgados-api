using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System.Net.Mime;

namespace SistemasSalgados.Bootstrapper
{
    public static class HealthCheckConfig
    {
        public static void ConfigHealthCheck(this IServiceCollection services, string connectionString)
        {
            services.AddHealthChecks()
                    .AddSqlServer(connectionString, name: "DataBaseSQL");
        }

        public static void StartHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/admin/healthcheck",
                new HealthCheckOptions()
                {
                    ResponseWriter = async (context, report) =>
                    {
                        var result = JsonConvert.SerializeObject(
                            new
                            {
                                statusApplication = report.Status.ToString(),
                                healthChecks = report.Entries.Select(e => new
                                {
                                    check = e.Key,
                                    ErrorMessage = e.Value.Exception?.Message,
                                    status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                                })
                            });

                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });
        }
    }
}
