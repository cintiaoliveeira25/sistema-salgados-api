using SistemasSalgados.Models.Base;

namespace SistemasSalgados.Models.Auth
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
