using SistemasSalgados.Models.Auth;
using SistemasSalgados.Models.Base;
using SistemasSalgados.Services.Interfaces.Base;

namespace SistemasSalgados.Services.Interfaces.Auth
{
    public interface IRefreshTokenService : IBaseService<RefreshToken>
    {
        Task<RefreshToken> SelectByIdAsync(int id);
        Task<Validation> DeleteAsync(int id);
        Task<Validation> Create(RefreshToken refreshToken);
        Task<RefreshToken> SelectByRefreshToken(string refreshToken);
        Task DeleteExpiredTokens();
    }
}
