using System.Net;

namespace WeShopAlot.Shared.Models
{
    /// <summary>
    /// Return OK WebAPI response with Result( status code = 200)
    /// </summary>
    public class OkResponse : BaseResponse
    {
        public object Result { get; }

        public OkResponse(object result)
            : base(HttpStatusCode.OK)
        {
            Result = result;
        }
    }
}
