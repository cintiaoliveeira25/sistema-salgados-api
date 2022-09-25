using Microsoft.EntityFrameworkCore;
using SistemasSalgados.Models.Base;
using SistemasSalgados.Persistence.Configuration.Context;
using SistemasSalgados.Services.Interfaces.Base;

namespace SistemasSalgados.Services.Entities.Base
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {
        protected readonly SistemaSalgadosDbContext _dbContext;
        public BaseService(SistemaSalgadosDbContext dbContext) => _dbContext = dbContext;

        public IQueryable<TEntity> Select()
        {
            return _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> SelectById(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<Validation> Insert(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            var insertedRows = await _dbContext.SaveChangesAsync();
            return new()
            {
                HasError = insertedRows <= 0
            };
        }

        public async Task<Validation> Insert(List<TEntity> entities)
        {
            try
            {
                _dbContext.Set<TEntity>().AddRange(entities);
                var insertedRows = await _dbContext.SaveChangesAsync();
                return new()
                {
                    HasError = insertedRows <= 0,
                    Messages = new() { "Não há linhas inseridas" }
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    HasError = true,
                    Messages = new() { ex.Message }
                };
            }
        }

        public async Task<Validation> Update(int id, TEntity entity)
        {
            try
            {
                TEntity currentEntity = await SelectById(id);

                if (currentEntity == null)
                    return new()
                    {
                        HasError = true,
                        Messages = new() { "Objeto não encontrado" }
                    };

                var currentEntityType = entity.GetType().FullName;

                if (_dbContext.Set<TEntity>().Local.All(p => p.GetType().FullName != currentEntityType))
                    _dbContext.Set<TEntity>().Attach(currentEntity);

                _dbContext.Entry(currentEntity).CurrentValues.SetValues(entity);

                if (_dbContext.Entry(currentEntity).State == EntityState.Unchanged)
                    return new()
                    {
                        HasError = false,
                        Messages = new() { "Não há propriedades alteradas" }
                    };

                _dbContext.Entry(currentEntity).Property(x => x.Created).IsModified = false;

                var affectedRows = await _dbContext.SaveChangesAsync();
                return new()
                {
                    HasError = affectedRows < 0,
                    Messages = new() { "Não há linhas alteradas" }
                };

            }
            catch (Exception ex)
            {
                return new()
                {
                    HasError = true,
                    Messages = new() { ex.Message }
                };
            }
        }

        public async Task<Validation> Delete(TEntity entity)
        {
            try
            {
                if (!EntryHasEntity(entity.GetType().FullName))
                    _dbContext.Set<TEntity>().Attach(entity);

                _dbContext.Entry(entity).State = EntityState.Deleted;
                var affectedRows = await _dbContext.SaveChangesAsync();

                return new()
                {
                    HasError = affectedRows < 0,
                    Messages = new() { "Não há linhas deletadas" }
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    HasError = true,
                    Messages = new() { ex.Message }
                };
            }

        }

        public async Task<Validation> DeleteById(int id)
        {
            TEntity entity = await SelectById(id);
            return await Delete(entity);
        }

        private bool EntryHasEntity(string entityName)
        {
            var hasAny = _dbContext.ChangeTracker
                .Entries<TEntity>()
                .Any(p => p.Entity.GetType().FullName == entityName);

            return hasAny;
        }
    }
}
