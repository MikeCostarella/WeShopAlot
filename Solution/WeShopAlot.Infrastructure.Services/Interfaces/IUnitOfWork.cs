using WeShopAlot.Data.Models.Base;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Infrastructure.Services.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BasePersistentObject;
        Task<int> Complete();
    }
}
