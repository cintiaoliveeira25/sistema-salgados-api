using SistemasSalgados.Models.Auth;
using SistemasSalgados.Models.Base;

namespace SistemasSalgados.Models
{
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public RoleEnum Role { get; set; }
    }
}
