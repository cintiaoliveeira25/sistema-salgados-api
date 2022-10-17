using SistemasSalgados.Models.Auth;
using SistemasSalgados.Services.Interfaces.Base;

namespace SistemasSalgados.Services.Interfaces.Auth
{
    public interface IUserRoleService : IBaseService<UserRole>
    {
        bool Insert(int id);
    }
}
