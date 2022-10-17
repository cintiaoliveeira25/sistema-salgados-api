using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemasSalgados.Models.Auth;
using SistemasSalgados.Persistence.Configuration.Base;

namespace SistemasSalgados.Persistence.Configuration.Auth
{
    public class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
    {
        public override void Configuration(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasOne(p => p.User).WithMany(p => p.UserRoles).HasForeignKey(p => p.UserId);
        }
    }
}
