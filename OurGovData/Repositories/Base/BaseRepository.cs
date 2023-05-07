using Microsoft.EntityFrameworkCore;
using MeShopAlot.Data.Model.Base;
using System.Linq.Expressions;

namespace MeShopAlot.Data.Repositories.Base
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : BasePersistentObject
    {
        #region Member Variables

        protected readonly MeShopAlotContext dbContext;

        #endregion Member Variables

        #region Constructors

        public BaseRepository(MeShopAlotContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #endregion Constructors

        #region Public Methods

        public Task<T> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> AddRangeAsync(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task CountByIdAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public virtual T GetById(int id) {
            return dbContext.Set<T>().FirstOrDefault(x => !x.IsDeleted && x.Id == id);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public virtual async Task<bool> IsExistByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> ListAllAsync()
        {
            throw new NotImplementedException();
        }

        public virtual async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T obj, params Expression<Func<T, object>>[] propertiesToUpdate)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(List<T> entities)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}
