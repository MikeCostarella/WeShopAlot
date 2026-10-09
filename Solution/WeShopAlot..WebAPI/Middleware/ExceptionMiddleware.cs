using WeShopAlot.WebAPI.Errors;
using System.Net;
using System.Text.Json;

namespace WeShopAlot.WebAPI.Middleware;

public class ExceptionMiddleware
{
    #region Member Variables

    private readonly RequestDelegate requestDelegate;
    private readonly ILogger<ExceptionMiddleware> logger;
    private readonly IHostEnvironment hostEnvironment;

    #endregion Member Variables

    #region Constructors

    public ExceptionMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger, IHostEnvironment hostEnvironment)
    {
        this.hostEnvironment = hostEnvironment;
        this.logger = logger;
        this.requestDelegate = requestDelegate;
    }

    #endregion Constructors

    #region Public Methods

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await requestDelegate(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = hostEnvironment.IsDevelopment()
                // GetBaseException digs out the real cause (such as the SQL error behind an EF
                // "See the inner exception for details" message) so it shows on the error page.
                ? new ApiException((int)HttpStatusCode.InternalServerError,
                    ex.GetBaseException() == ex ? ex.Message : $"{ex.Message} Cause: {ex.GetBaseException().Message}",
                    ex.StackTrace?.ToString())
                : new ApiException((int)HttpStatusCode.InternalServerError);
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }

    #endregion Public Methods
}
