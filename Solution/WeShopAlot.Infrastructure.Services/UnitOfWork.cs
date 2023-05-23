using WeShopAlot.Data;
using WeShopAlot.Data.Models.Base;
using WeShopAlot.Data.Repositories.Base;
using WeShopAlot.Infrastructure.Services.Interfaces;
using System.Collections;

namespace WeShopAlot.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WeShopAlotContext _context;
        private Hashtable _repositories;
        public UnitOfWork(WeShopAlotContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BasePersistentObject
        {
            if (_repositories == null) _repositories = new Hashtable();
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(BaseRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }
            return (IAsyncRepository<TEntity>)_repositories[type];
        }
    }
}
