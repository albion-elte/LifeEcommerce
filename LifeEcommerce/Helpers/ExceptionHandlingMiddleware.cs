using Newtonsoft.Json;
using System.Net;

namespace LifeEcommerce.Helpers
{
    public class ExceptionHandlingMiddleware
    {
        public RequestDelegate _requestDelegate;
        public readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context) 
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception e) 
            {
                await HandleException(context, e);
            }
        }

        public Task HandleException(HttpContext context, Exception ex) 
        {
            _logger.LogError(ex.ToString());

            var errorMessage = new 
            {
                Message = ex.Message,
                Code = "Error",
                StatusCode = context.Response.StatusCode
            };

            var costumResponse = JsonConvert.SerializeObject(errorMessage);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(costumResponse);
        }
    }
}
