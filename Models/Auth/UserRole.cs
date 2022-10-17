using SistemasSalgados.Models.Base;

namespace SistemasSalgados.Models.Auth
{
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public RoleEnum Role { get; set; }
    }
}
