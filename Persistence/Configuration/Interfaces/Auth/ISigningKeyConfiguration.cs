using Microsoft.IdentityModel.Tokens;

namespace SistemasSalgados.Persistence.Configuration.Interfaces.Auth
{
    public interface ISigningKeyConfiguration
    {
        SigningCredentials SigningCredentials { get; }
        SecurityKey SecurityKey { get; }
    }
}
