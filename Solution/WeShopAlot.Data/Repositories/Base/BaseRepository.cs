using Microsoft.EntityFrameworkCore;
using WeShopAlot.Data.Models.Base;
using System.Linq.Expressions;
using WeShopAlot.Data.Specifications.Base;
using WeShopAlot.Data.Utilities.Specifications;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace WeShopAlot.Data.Repositories.Base
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : BasePersistentObject
    {
        #region Member Variables

        protected readonly WeShopAlotContext dbContext;

        #endregion Member Variables

        #region Constructors

        public BaseRepository(WeShopAlotContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #endregion Constructors

        #region Public Methods

        public void Add(T entity)
        {
            dbContext.Set<T>().Add(entity);
        }

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

        public virtual T GetById(int id)
        {
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
            await Task.Delay(0);
            throw new NotImplementedException();
        }

        public List<T> ListAll()
        {
            return dbContext.Set<T>().Where(x => !x.IsDeleted).ToList();
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

        public void Update(T entity)
        {
            dbContext.Set<T>().Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task UpdateAsync(T entity)
        {
            var EntityEntry = dbContext.Set<T>().Attach(entity);
            EntityEntry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public Task UpdateAsync(T obj, params Expression<Func<T, object>>[] propertiesToUpdate)
        {
            var EntityEntry = dbContext.Set<T>().Attach(obj);
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
