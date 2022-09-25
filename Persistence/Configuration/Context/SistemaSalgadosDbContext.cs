using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace SistemasSalgados.Persistence.Configuration.Context
{
    public class SistemaSalgadosDbContext : DbContext
    {
        public SistemaSalgadosDbContext(DbContextOptions<SistemaSalgadosDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
