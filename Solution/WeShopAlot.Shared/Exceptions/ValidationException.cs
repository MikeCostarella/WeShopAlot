using System.Net;

namespace WeShopAlot.Shared.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        private readonly List<ErrorDetail> _errors;

        public HttpStatusCode StatusCode { get; set; }

        public ValidationException(string propertyName, string message)
        {
            _errors = Errors;
            StatusCode = HttpStatusCode.BadRequest;
            _errors.Add(new ErrorDetail(propertyName, message));
        }

        public ValidationException()
        {
            StatusCode = HttpStatusCode.BadRequest;
            _errors = Errors;
        }

        public ValidationException(List<ErrorDetail> errors)
        {
            StatusCode = HttpStatusCode.BadRequest;
            _errors = errors;
        }

        public List<ErrorDetail> Errors
        {
            get
            {
                return _errors ?? new List<ErrorDetail>();
            }
        }
    }
}
