using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//using WeShopAlot.Shared.Exceptions;
//using WeShopAlot.Shared.Extensions;
//using WeShopAlot.Shared.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LL.Shared.Services.ServiceAPIClient
{
    public class ServiceAPIClient : IServiceAPIClient
    {
        #region Member Variables

        private readonly IConfiguration _configuration;
        private readonly ILogger<ServiceAPIClient> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        #endregion Member Variables

        #region Constructors

        public ServiceAPIClient(IConfiguration configuration,
            ILogger<ServiceAPIClient> logger,
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            var baseUrl = _configuration["ApiPath"];
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient = httpClient;
        }

        #endregion Constructors

        #region Actions

        /// <summary>
        /// Make a Get call to web api
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string requestUri)
        {
            AddJWTToAuthorizationHeader();
            HttpResponseMessage _response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false);
            return await ReturnObjectAsync<T>(_response).ConfigureAwait(false);
        }

        /// <summary>
        /// Make a Post call to webapi
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="stringContent"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string requestUri, StringContent stringContent)
        {
            AddJWTToAuthorizationHeader();
            HttpResponseMessage _response = await _httpClient.PostAsync(requestUri, stringContent).ConfigureAwait(false);
            return await ReturnObjectAsync<T>(_response).ConfigureAwait(false);
        }

        /// <summary>
        /// Make a Post call to webapi
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="stringContent"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string requestUri, MultipartFormDataContent multipartFormDataContent)
        {
            AddJWTToAuthorizationHeader();
            HttpResponseMessage _response = await _httpClient.PostAsync(requestUri, multipartFormDataContent).ConfigureAwait(false);
            return await ReturnMultipartFormDataObjectAsync<T>(_response).ConfigureAwait(false);
        }

        /// <summary>
        /// Make a post call to webapi for the MultiFormDataContent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="multipartFormDataContent"></param>
        /// <returns></returns>
        public async Task<string> PostAsync(string requestUri, MultipartFormDataContent multipartFormDataContent)
        {
            AddJWTToAuthorizationHeader();
            HttpResponseMessage _response = await _httpClient.PostAsync(requestUri, multipartFormDataContent).ConfigureAwait(false);
            return await ReturnMultipartFormDataObjectAsync(_response).ConfigureAwait(false);
        }

        /// <summary>
        /// Make a PUT call to Web api
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="stringContent"></param>
        /// <returns></returns>
        public async Task<T> PutAsync<T>(string requestUri)
        {
            return await PutAsync<T>(requestUri, null);
        }

        /// <summary>
        /// Make a PUT call to Web api
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="stringContent"></param>
        /// <returns></returns>
        public async Task<T> PutAsync<T>(string requestUri, StringContent stringContent)
        {
            try
            {
                AddJWTToAuthorizationHeader();
                HttpResponseMessage _response = await _httpClient.PutAsync(requestUri, stringContent).ConfigureAwait(false);
                return await ReturnObjectAsync<T>(_response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Add JWT token from Session to request authorization header
        /// </summary>
        private void AddJWTToAuthorizationHeader()
        {
            string token;
            //Ensure the HttpContext is not null
            if (_httpContextAccessor.HttpContext != null)
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    token = _httpContextAccessor.HttpContext.User.FindFirst(Constants.JWT_Token_Name)?.Value;
                }
                else
                {
                    token = _httpContextAccessor.HttpContext.Session.GetString(Constants.JWT_Token_Name);
                }
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
        }

        /// <summary>
        /// De-seralize string back to Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_response"></param>
        /// <returns></returns>
        private async Task<T> ReturnObjectAsync<T>(HttpResponseMessage _response)
        {
            if (_response.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                string result = await _response.Content.ReadAsStringAsync().ConfigureAwait(false);
                T response = default;
                if (!string.IsNullOrEmpty(result))
                {
                    response = JsonSerializerExtension.DeserializeOKResponse<T>(result);
                }
                return response;
            }
            else
            {
                string result = _response.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(result))
                {
                    throw new HttpStatusCodeException(_response.StatusCode, "WebAPI client response return " +
                        _response.StatusCode.GetDescription() + " Error. Please contact System adminiatrator for more details.");
                }
                var response = JsonSerializerExtension.DeserializeObject<BaseResponse>(result);
                //re-throw the exception from API project
                if (response.ValidationErrors != null)
                {
                    _logger.LogInformation("WebAPI client response validator errors found. Error: " + response.ValidationErrors.FirstOrDefault().Message);
                    throw new HttpStatusCodeException(response.StatusCode, response.ValidationErrors.FirstOrDefault().Message);
                }
                else
                {
                    _logger.LogInformation("No WebAPI client response validator errors found. Error message is found: " + response.Message);
                    throw new HttpStatusCodeException(response.StatusCode, response.Message);
                }
            }
        }

        private async Task<string> ReturnMultipartFormDataObjectAsync(HttpResponseMessage _response)
        {
            if (_response.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                string result = await _response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return result;
            }
            else
            {
                string result = _response.Content.ReadAsStringAsync().Result;

                //if (string.IsNullOrEmpty(result))
                //{
                //    throw new HttpStatusCodeException(_response.StatusCode, "WebAPI client response return " +
                //        _response.StatusCode.GetDescription() + " Error. Please contact System adminiatrator for more details.");
                //}

                //var response = JsonSerializerExtension.DeserializeObject<BaseResponse>(result);
                ////re-throw the exception from API project
                //if (response.ValidationErrors != null)
                //{
                //    _logger.LogInformation("WebAPI client response validator errors found. Error: " + response.ValidationErrors.FirstOrDefault().Message);
                //    throw new HttpStatusCodeException(response.StatusCode, response.ValidationErrors.FirstOrDefault().Message);
                //}
                //else
                //{
                //    _logger.LogInformation("No WebAPI client response validator errors found. Error message is found: " + response.Message);
                //    throw new HttpStatusCodeException(response.StatusCode, response.Message);
                //}
            }
        }

        private async Task<T> ReturnMultipartFormDataObjectAsync<T>(HttpResponseMessage _response)
        {
            if (_response.IsSuccessStatusCode)
            {
                T response = default;
                //Storing the response details recieved from web api     
                string result = await _response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrEmpty(result))
                {
                    //response = JsonSerializerExtension.DeserializeOKResponse<T>(result);
                }
                return response;
            }
            else
            {
                string result = _response.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(result))
                {
                    throw new HttpStatusCodeException(_response.StatusCode, "WebAPI client response return " +
                        _response.StatusCode.GetDescription() + " Error. Please contact System adminiatrator for more details.");
                }
                var response = JsonSerializerExtension.DeserializeObject<BaseResponse>(result);
                //re-throw the exception from API project
                if (response.ValidationErrors != null)
                {
                    _logger.LogInformation("WebAPI client response validator errors found. Error: " + response.ValidationErrors.FirstOrDefault().Message);
                    throw new HttpStatusCodeException(response.StatusCode, response.ValidationErrors.FirstOrDefault().Message);
                }
                else
                {
                    _logger.LogInformation("No WebAPI client response validator errors found. Error message is found: " + response.Message);
                    throw new HttpStatusCodeException(response.StatusCode, response.Message);
                }
            }

        }

        #endregion Actions
    }
}
