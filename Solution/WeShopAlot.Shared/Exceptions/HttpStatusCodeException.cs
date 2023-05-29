using Newtonsoft.Json.Linq;
using System.Net;

namespace WeShopAlot.Shared.Exceptions
{
    /// <summary>
    /// Create a new HttpStatusCode exception
    /// hrow whenever I wanted to stop execution of the current request and send back a response with a specific Status Code and Body
    /// https://blogs.msdn.microsoft.com/brandonh/2017/07/31/using-middleware-to-trap-exceptions-in-asp-net-core/
    /// </summary>
    public class HttpStatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ContentType { get; set; } = @"text/plain";

        public HttpStatusCodeException(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCodeException(HttpStatusCode statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCodeException(HttpStatusCode statusCode, Exception inner) : this(statusCode, inner.ToString()) { }

        public HttpStatusCodeException(HttpStatusCode statusCode, JObject errorObject) : this(statusCode, errorObject.ToString())
        {
            this.ContentType = @"application/json";
        }
    }
}
