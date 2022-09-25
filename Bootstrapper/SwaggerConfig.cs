namespace SistemasSalgados.Bootstrapper
{
    public static class SwaggerConfig
    {
        public static void ConfigSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new()
                {
                    Title = "Sistema Salgados",
                    Version = "v1",
                    Description = "API Sistema Salgados"
                });
            });
        }

        public static void StartSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/Swagger/v1/swagger.json", "Sistema Salgados");
                config.RoutePrefix = "admin/swagger";
            });
        }
    }
}
