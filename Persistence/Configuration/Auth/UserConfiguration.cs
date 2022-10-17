using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemasSalgados.Models.Auth;
using SistemasSalgados.Persistence.Configuration.Base;

namespace SistemasSalgados.Persistence.Configuration.Auth
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configuration(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(x => x.UserRoles).WithOne(p => p.User).HasForeignKey(p => p.UserId);
        }
    }
}
