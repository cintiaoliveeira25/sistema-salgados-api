using SistemasSalgados.Models.Auth;

namespace SistemasSalgados.Services.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<AuthenticatedUser> AuthenticateWithRefreshToken(Guid token);
        Task<AuthenticatedUser> AuthenticateWithPassword(string password, User user);
    }
}
