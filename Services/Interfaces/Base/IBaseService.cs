using SistemasSalgados.Models.Base;

namespace SistemasSalgados.Services.Interfaces.Base
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> Select();
        Task<TEntity> SelectById(int id);
        Task<Validation> Insert(TEntity entity);
        Task<Validation> Insert(List<TEntity> entities);
        Task<Validation> Update(int id, TEntity entity);
        Task<Validation> Delete(TEntity entity);
        Task<Validation> DeleteById(int id);
    }
}
