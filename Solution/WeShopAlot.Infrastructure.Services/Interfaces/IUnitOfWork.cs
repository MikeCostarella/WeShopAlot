using WeShopAlot.Data.Model.Base;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Infrastructure.Services.Interfaces
{
    public interface IUnitOfWork
    {
        IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BasePersistentObject;
        Task<int> Complete();
    }
}
