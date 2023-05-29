using WeShopAlot.Shared.Extensions;
using System.Net;

namespace WeShopAlot.Shared.Exceptions
{
    public class ErrorDetail
    {
        /// <summary>
        /// System exception detail
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        public ErrorDetail(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
            Property = string.Empty;
        }

        /// <summary>
        /// Validation error detail
        /// </summary>
        /// <param name="property"></param>
        /// <param name="message"></param>
        public ErrorDetail(string property, string message)
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
            Message = message;
            Property = property;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Property { get; set; }

        public override string ToString()
        {
            return JsonSerializerExtension.SerializeObject(this);
        }
    }
}
