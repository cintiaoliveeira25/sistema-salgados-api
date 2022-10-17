using System.ComponentModel.DataAnnotations;

namespace SistemasSalgados.Dtos
{
    public class LoginDto
    {
        //public string AccessToken { get; set; }
        //public Guid RefreshToken { get; set; }
        public string GrantType { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid RefreshToken { get; set; }

        //[Required, EmailAddress]
        //public string Email { get; set; }
        //[Required]
        //public string Password { get; set; }
    }
}
