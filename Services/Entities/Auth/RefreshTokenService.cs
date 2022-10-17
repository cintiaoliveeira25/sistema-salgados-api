using Microsoft.EntityFrameworkCore;
using SistemasSalgados.Models.Auth;
using SistemasSalgados.Models.Base;
using SistemasSalgados.Persistence.Configuration.Context;
using SistemasSalgados.Services.Entities.Base;
using SistemasSalgados.Services.Interfaces.Auth;

namespace SistemasSalgados.Services.Entities.Auth
{
    public class RefreshTokenService : BaseService<RefreshToken>, IRefreshTokenService
    {
        public RefreshTokenService(SistemaSalgadosDbContext dbContext) : base(dbContext)
        { }

        public async Task<Validation> Create(RefreshToken refreshToken)
        {
            return await Insert(refreshToken);
        }

        public async Task<Validation> DeleteAsync(int id)
        {
            return await DeleteById(id);
        }

        public async Task DeleteExpiredTokens()
        {
            var refreshTokenDbContext = _dbContext.Set<RefreshToken>();

            var listToBeDeleted = await refreshTokenDbContext.Where(x => x.ExpiresIn < DateTime.UtcNow).ToListAsync();

            refreshTokenDbContext.RemoveRange(listToBeDeleted);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken> SelectByIdAsync(int id)
        {
            return await SelectById(id);
        }

        public async Task<RefreshToken> SelectByRefreshToken(string refreshToken)
        {
            return await _dbContext.Set<RefreshToken>().Where(p => string.Equals(p.Token.ToString(), refreshToken)).FirstOrDefaultAsync();
        }
    }
}
