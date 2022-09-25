using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemasSalgados.Models.Base;

namespace SistemasSalgados.Persistence.Configuration.Base
{
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Created)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.Updated)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETDATE()");

            Configuration(builder);
        }

        public abstract void Configuration(EntityTypeBuilder<TEntity> builder);
    }
}
