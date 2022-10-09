using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemasSalgados.Models;
using SistemasSalgados.Persistence.Configuration.Base;

namespace SistemasSalgados.Persistence.Configuration.Auth
{
    public class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
    {
        public override void Configuration(EntityTypeBuilder<UserRole> builder)
        { }
    }
}
