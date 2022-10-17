namespace SistemasSalgados.Persistence.Configuration.Interfaces.Auth
{
    public interface ITokenConfiguration
    {
        string SecretKey { get; set; }
        string Issuer { get; set; }
        string Audience { get; set; }
        int DefaultExpiresIn { get; set; }
        int RefreshTokenExpiresIn { get; set; }
    }
}
