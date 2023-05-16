using WeShopAlot.Data.Model.Base;
using WeShopAlot.Data.Specifications.Base;
using System.Linq.Expressions;

namespace WeShopAlot.Data.Repositories.Base
{
    public interface IAsyncRepository<T> where T : BasePersistentObject
    {
        void Add(T entity);

        Task<T> AddAsync(T entity);

        Task<T> AddRangeAsync(List<T> entities);

        Task<int> CountAsync(ISpecification<T> spec);

        Task CountByIdAsync();

        Task DeleteAsync(T entity);

        Task DeleteByIdAsync(int id);

        Task<T> GetByIdAsync(int id);

        Task<T> GetEntityWithSpec(ISpecification<T> spec);

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        Task<List<T>> ListAllAsync();

        void Update(T entity);

        Task UpdateAsync(T entity);

        Task UpdateRangeAsync(List<T> entities);

        Task UpdateAsync(T obj, params Expression<Func<T, object>>[] propertiesToUpdate);

        Task SaveChangesAsync();

        Task<bool> IsExistByIdAsync(int id);
    }
}
