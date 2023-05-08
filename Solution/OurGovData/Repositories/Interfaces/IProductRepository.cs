using MeShopAlot.Data.Models;
using MeShopAlot.Data.Repositories.Base;

namespace MeShopAlot.Data.Repositories.Interfaces
{
    public interface IProductRepository : IAsyncRepository<Product>
    {
    }
}
