namespace SistemasSalgados.Models.Auth
{
    public class WebToken
    {
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
