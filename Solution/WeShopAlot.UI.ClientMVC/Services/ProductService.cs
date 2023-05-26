using WeShopAlot.UI.ClientMVC.Models;

namespace WeShopAlot.UI.ClientMVC.Services
{
    public class ProductService : IProductService
    {
        public ProductService() {
        }

        public List<ProductViewModel> GetProducts()
        {
            string requestUri = Constants.WebAPI_Product_GetProducts;
            //var httpContent = JsonSerializerExtension.SerializeObjectToStringContent(applicationToSave);
            //var response = await _apiClient.GetAsync<ProductToReturnDto>(requestUri, httpContent).ConfigureAwait(false);
            //return _mapper.Map<ProductViewModel>(response);
            return new List<ProductViewModel>();
        }
    }
}
