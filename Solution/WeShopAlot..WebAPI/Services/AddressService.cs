using AutoMapper;
using WeShopAlot.WebAPI.Services.Base;
using WeShopAlot.Shared.Services.ServiceAPIClient;

namespace WeShopAlot.WebAPI.Services
{
    public class AddressService : BaseService
    {
        //_apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        //private readonly IWebHostEnvironment _host;

        //public AddressService(IWebHostEnvironment host,  IServiceAPIClient apiClient)
        //{
        //    _host = host ?? throw new ArgumentNullException(nameof(host));
        //    _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        //}


        //public async Task<List<ApplicationViewModel>> GetApplicationsInCart(int userId)
        //{
        //    var requestUri = Constants.WebAPI_Application_GetUserApplicationsInCart;
        //    requestUri += "?registeredUserId=" + userId + "&entryStatusId=" + (int)ApplicationEntryStatusEnum.InCart;
        //    var response = await _apiClient.GetAsync<List<ApplicationDto>>(requestUri).ConfigureAwait(false);
        //    return _mapper.Map<List<ApplicationViewModel>>(response);
        //}
    }
}
