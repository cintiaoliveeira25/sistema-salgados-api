using SistemasSalgados.Models;
using SistemasSalgados.Persistence.Configuration.Context;
using SistemasSalgados.Services.Entities.Base;
using SistemasSalgados.Services.Interfaces.Auth;

namespace SistemasSalgados.Services.Entities.Auth
{
    public class UserRoleService : BaseService<UserRole>, IUserRoleService
    {
        public UserRoleService(SistemaSalgadosDbContext dbContext) : base(dbContext)
        { }
    }
}
