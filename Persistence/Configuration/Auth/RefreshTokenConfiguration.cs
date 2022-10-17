using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemasSalgados.Models.Auth;
using SistemasSalgados.Persistence.Configuration.Base;

namespace SistemasSalgados.Persistence.Configuration.Auth
{
    class RefreshTokenConfiguration : BaseEntityConfiguration<RefreshToken>
    {
        public override void Configuration(EntityTypeBuilder<RefreshToken> builder)
        { }
    }
}
