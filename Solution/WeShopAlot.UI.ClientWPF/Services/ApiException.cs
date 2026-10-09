using System.Net;
using System.Net.Http;

namespace WeShopAlot.UI.ClientWPF.Services
{
    /// <summary>
    /// An error answer from the API. The API sends { statusCode, message } (ApiResponse) and, for validation
    /// failures, an errors list (ApiValidationErrorResponse); both end up here.
    /// </summary>
    public class ApiException : HttpRequestException
    {
        public ApiException(string message, HttpStatusCode statusCode, IReadOnlyList<string>? errors = null)
            : base(message, null, statusCode)
        {
            Errors = errors ?? Array.Empty<string>();
        }

        public IReadOnlyList<string> Errors { get; }

        /// <summary>The message plus any validation errors, one per line.</summary>
        public string Details => Errors.Count == 0 ? Message : string.Join(Environment.NewLine, Errors);
    }
}
