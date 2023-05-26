using Microsoft.VisualBasic;
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
            //var applicationToSave = _mapper.Map<ApplicationDto>(applicationViewModel);
            //var httpContent = JsonSerializerExtension.SerializeObjectToStringContent(applicationToSave);
            //var response = await _apiClient.PostAsync<ApplicationDto>(requestUri, httpContent).ConfigureAwait(false);
            //return _mapper.Map<ApplicationViewModel>(response);
            return new List<ProductViewModel>();
        }
    }
}
