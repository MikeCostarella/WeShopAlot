using WeShopAlot.UI.ClientMVC.Models;

namespace WeShopAlot.UI.ClientMVC.Services
{
    public interface IProductService
    {
        public List<ProductViewModel> GetProducts();

        public ProductViewModel GetProductDetail(int id);

        public List<ProductViewModel> GetTrendingProducts();
    }
}
