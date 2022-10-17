using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SistemasSalgados.Persistence.Configuration.Auth;
using SistemasSalgados.Persistence.Configuration.Interfaces.Auth;
using System.Text;

namespace SistemasSalgados.Bootstrapper
{
    public static class Authentication
    {
        public static void AddApplicationAuth(this IServiceCollection services, IConfiguration configuration)
        {
            ITokenConfiguration tokenConfiguration = new TokenConfiguration();

            new ConfigureFromConfigurationOptions<ITokenConfiguration>(
                    configuration.GetSection("TokenConfiguration"))
                .Configure(tokenConfiguration);

            if (string.IsNullOrEmpty(tokenConfiguration.SecretKey))
                throw new InvalidOperationException("Chave secreta de token ausente.");

            byte[] key = Encoding.UTF8.GetBytes(tokenConfiguration.SecretKey);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

            services.AddSingleton(tokenConfiguration);

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var validationParameters = options.TokenValidationParameters;

                validationParameters.ValidateAudience = true;
                validationParameters.ValidateIssuer = true;
                validationParameters.ClockSkew = TimeSpan.Zero;

                validationParameters.ValidAudience = tokenConfiguration.Audience;
                validationParameters.ValidIssuer = tokenConfiguration.Issuer;
                validationParameters.RequireExpirationTime = true;

                validationParameters.IssuerSigningKey = new SymmetricSecurityKey(key);
            });
        }

        public static void UseApplicationAuth(this IApplicationBuilder app)
        {
            app.UseAuthorization();
            app.UseAuthentication();
        }
    }
}
