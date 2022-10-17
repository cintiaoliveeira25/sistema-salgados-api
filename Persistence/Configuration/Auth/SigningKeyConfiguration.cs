using Microsoft.IdentityModel.Tokens;
using SistemasSalgados.Persistence.Configuration.Interfaces.Auth;
using System.Text;

namespace SistemasSalgados.Persistence.Configuration.Auth
{
    public class SigningKeyConfiguration : ISigningKeyConfiguration
    {
        public SigningCredentials SigningCredentials { get; }
        public SecurityKey SecurityKey { get; }

        public SigningKeyConfiguration(ITokenConfiguration tokenConfiguration)
        {
            byte[] key = Encoding.UTF8.GetBytes(tokenConfiguration.SecretKey);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

            SecurityKey = securityKey;

            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        }
    }
}
