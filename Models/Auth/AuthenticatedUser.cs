namespace SistemasSalgados.Models.Auth
{
    public class AuthenticatedUser
    {
        public User User { get; set; }
        public WebToken WebToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
