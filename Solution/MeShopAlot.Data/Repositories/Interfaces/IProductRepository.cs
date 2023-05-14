using MeShopAlot.Data.Models;
using MeShopAlot.Data.Repositories.Base;

namespace MeShopAlot.Data.Repositories.Interfaces
{
    public interface IProductRepository : IAsyncRepository<Product>
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IReadOnlyList<Product>> GetProductsAsync();
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
    }
}
