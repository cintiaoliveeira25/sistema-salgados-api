using SistemasSalgados.Models.Auth;
using SistemasSalgados.Persistence.Configuration.Context;
using SistemasSalgados.Services.Entities.Base;
using SistemasSalgados.Services.Interfaces.Auth;

namespace SistemasSalgados.Services.Entities.Auth
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(SistemaSalgadosDbContext dbContext) : base(dbContext)
        { }
    }
}
