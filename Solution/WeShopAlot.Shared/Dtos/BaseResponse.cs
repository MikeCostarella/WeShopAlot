using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using WeShopAlot.Shared.Exceptions;
using System.Net;

namespace WeShopAlot.Shared.Models
{
    /// <summary>
    /// Return WebAPI base response object  including Statuscode, Message and/or Business validation errors
    /// https://www.jerriepelser.com/blog/validation-response-aspnet-core-webapi/
    /// </summary>
    public class BaseResponse
    {
        public HttpStatusCode StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public string HostName { get; set; }
        public List<ErrorDetail> ValidationErrors { get; set; }

        public BaseResponse(HttpStatusCode statusCode, ModelStateDictionary modelState, string hostName = null)
        {
            StatusCode = statusCode;
            Message = "Validation Failed";
            HostName = hostName;
            ValidationErrors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ErrorDetail(key, x.ErrorMessage)))
                    .ToList();
        }

        public BaseResponse(HttpStatusCode statusCode, string message = null, string hostName = null)
        {
            HostName = hostName;
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode((int)statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            //return statusCode.ToString();
            switch (statusCode)
            {
                case 200:
                    return "OK";
                case 302:
                    return "Found";
                case 400:
                    return "Bad Request";
                case 401:
                    return "Unauthorized";
                case 403:
                    return "Forbidden";
                case 404:
                    return "Resource not found";
                case 415:
                    return "Unsupported Media Type";
                case 500:
                    return "Internal Server Error";
                case 502:
                    return "Bad Gateway";
                case 503:
                    return "Service Unavailable";
                case 504:
                    return "Gateway Timeout";
                default:
                    return null;
            }
        }
    }
}
