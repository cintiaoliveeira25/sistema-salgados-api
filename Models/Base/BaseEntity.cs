namespace SistemasSalgados.Models.Base
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
