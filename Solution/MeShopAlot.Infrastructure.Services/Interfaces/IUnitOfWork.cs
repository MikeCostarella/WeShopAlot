using MeShopAlot.Data.Model.Base;
using MeShopAlot.Data.Repositories.Base;

namespace MeShopAlot.Infrastructure.Services.Interfaces
{
    public interface IUnitOfWork
    {
        IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BasePersistentObject;
        Task<int> Complete();
    }
}
