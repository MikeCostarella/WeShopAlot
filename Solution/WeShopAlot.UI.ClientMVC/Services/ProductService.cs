using AutoMapper;
using WeShopAlot.Shared.Dtos;
using WeShopAlot.Shared.Extensions;
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
            //var httpContent = JsonSerializerExtension.SerializeObjectToStringContent(applicationToSave);
            //var response = await _apiClient.GetAsync<ProductToReturnDto>(requestUri, httpContent).ConfigureAwait(false);
            var response =  _apiClient.GetAsync<List<ProductToReturnDto>>(requestUri).ConfigureAwait(false);
            return _mapper.Map <List<ProductViewModel>>(response);
        }
    }
}
