using MeShopAlot.Data.Model.Base;
using System.Linq.Expressions;

namespace MeShopAlot.Data.Repositories.Base
{
    public interface IAsyncRepository<T> where T : BasePersistentObject
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> AddRangeAsync(List<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(List<T> entities);
        Task UpdateAsync(T obj, params Expression<Func<T, object>>[] propertiesToUpdate);
        Task DeleteAsync(T entity);
        Task DeleteByIdAsync(int id);
        Task SaveChangesAsync();
        Task CountByIdAsync();
        Task<bool> IsExistByIdAsync(int id);
    }
}
