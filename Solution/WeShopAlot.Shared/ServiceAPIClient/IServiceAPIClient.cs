using System.Net.Http;
using System.Threading.Tasks;

namespace WeShopAlot.Shared.Services.ServiceAPIClient
{
    public interface IServiceAPIClient
    {
        Task<T> GetAsync<T>(string requestUri);

        Task<T> PostAsync<T>(string requestUri, StringContent stringContent);

        Task<string> PostAsync(string requestUri, MultipartFormDataContent multipartFormDataContent);

        Task<T> PostAsync<T>(string requestUri, MultipartFormDataContent multipartFormDataContent);

        Task<T> PutAsync<T>(string requestUri);

        Task<T> PutAsync<T>(string requestUri, StringContent stringContent);
    }
}
