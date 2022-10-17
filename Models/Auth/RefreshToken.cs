using SistemasSalgados.Models.Base;

namespace SistemasSalgados.Models.Auth
{
    public class RefreshToken : BaseEntity
    {
        public Guid RefreshTokenId { get; set; }
        public Guid Token { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}
