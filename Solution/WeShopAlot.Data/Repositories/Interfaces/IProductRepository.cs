using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Data.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Product GetProductById(int id);
        Task<Product> GetProductByIdAsync(int id);
        IReadOnlyList<Product> GetProducts();
        Task<IReadOnlyList<Product>> GetProductsAsync();
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
    }
}
