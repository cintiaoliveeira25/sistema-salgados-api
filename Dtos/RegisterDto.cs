using System.ComponentModel.DataAnnotations;

namespace SistemasSalgados.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "O nome do usuário é obrigatório", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required, EmailAddress(ErrorMessage = "Digite um endereço de e-mail válido!")]
        public string Email { get; set; }

        [Required, MinLength(6, ErrorMessage = "Por favor adicione pelo menos 6 caracteres.")]
        public string Password { get; set; }

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
