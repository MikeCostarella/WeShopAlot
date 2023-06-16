using AutoMapper;
using WeShopAlot.Shared.Dtos;
using WeShopAlot.Shared.Services.ServiceAPIClient;
using WeShopAlot.UI.ClientMVC.Models;

namespace WeShopAlot.UI.ClientMVC.Services
{
    public class ProductService : IProductService
    {
        private readonly IServiceAPIClient _apiClient;
        private readonly IMapper _mapper;
        public ProductService(IMapper mapper, IServiceAPIClient apiClient) 
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        public List<ProductViewModel> GetProducts()
        {
            string requestUri = Constants.WebAPI_Product_GetProducts;
            var response =  _apiClient.GetAsync<List<ProductToReturnDto>>(requestUri).ConfigureAwait(false);
            return _mapper.Map <List<ProductViewModel>>(response);
        }

        public ProductViewModel GetProductDetail(int id)
        {
            string requestUri = Constants.WebAPI_Product_GetProductDetail;
            var response = _apiClient.GetAsync<ProductToReturnDto>(requestUri + "?id=" + id).ConfigureAwait(false);
            return _mapper.Map<ProductViewModel>(response);
        }

        public List<ProductViewModel> GetTrendingProducts()
        {
            string requestUri = Constants.WebAPI_Product_GetTrendingProducts;
            var response = _apiClient.GetAsync<List<ProductToReturnDto>>(requestUri).ConfigureAwait(false);
            return _mapper.Map<List<ProductViewModel>>(response);
        }

        //var httpContent = JsonSerializerExtension.SerializeObjectToStringContent(applicationToSave);
        //var response = await _apiClient.GetAsync<ProductToReturnDto>(requestUri, httpContent).ConfigureAwait(false);


    }
}
