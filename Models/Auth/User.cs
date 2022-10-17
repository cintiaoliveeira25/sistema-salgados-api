using SistemasSalgados.Models.Base;

namespace SistemasSalgados.Models.Auth
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
