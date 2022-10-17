using SistemasSalgados.Models.Auth;
using SistemasSalgados.Persistence.Configuration.Context;
using SistemasSalgados.Services.Entities.Base;
using SistemasSalgados.Services.Interfaces.Auth;

namespace SistemasSalgados.Services.Entities.Auth
{
    public class UserRoleService : BaseService<UserRole>, IUserRoleService
    {
        public UserRoleService(SistemaSalgadosDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Insert(int userId)
        {
            try
            {
                var userRole = new UserRole
                {
                    UserId = userId,
                    Role = RoleEnum.Usuario,
                    IsActive = true,
                };

                _dbContext.Set<UserRole>().Add(userRole);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
