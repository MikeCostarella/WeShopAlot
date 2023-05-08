using Microsoft.EntityFrameworkCore;
using MeShopAlot.Data.Model.Base;
using System.Linq.Expressions;
using MeShopAlot.Data.Specifications.Base;
using MeShopAlot.Data.Utilities.Specifications;

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

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
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

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public virtual async Task<bool> IsExistByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await dbContext.Set<T>().Where(x => !x.IsDeleted).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public virtual async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            var EntityEntry = dbContext.Set<T>().Attach(entity);
            EntityEntry.State = EntityState.Modified;
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public Task UpdateAsync(T obj, params Expression<Func<T, object>>[] propertiesToUpdate)
        {
            //var EntityEntry = dbContext.Set<T>().Attach(obj);
            //var members = propertiesToUpdate.SelectMany(selector => selector.GetPropertyAccesses());
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(List<T> entities)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Private Methods

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(dbContext.Set<T>().AsQueryable(), spec);
        }

        #endregion Private Methods
    }
}
