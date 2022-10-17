using SistemasSalgados.Persistence.Configuration.Interfaces.Auth;

namespace SistemasSalgados.Persistence.Configuration.Auth
{
    public class TokenConfiguration : ITokenConfiguration
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int DefaultExpiresIn { get; set; }
        public int RefreshTokenExpiresIn { get; set; }
    }
}
