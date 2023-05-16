using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;
using WeShopAlot.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WeShopAlot.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        #region Constructors

        public ProductRepository(WeShopAlotContext context) : base(context) { }

        #endregion Constructors

        #region Public Methods

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await dbContext.ProductBrands.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await dbContext.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await dbContext.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await dbContext.ProductTypes.ToListAsync();
        }

        #endregion Public Methods
    }
}
