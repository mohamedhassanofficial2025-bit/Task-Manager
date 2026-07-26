

namespace TaskManager.API.Middlewares;
public class ExceptionHandlingMiddleware
{
    /*-------------------------------------------------------------------------*/
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    /*-------------------------------------------------------------------------*/
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    /*-------------------------------------------------------------------------*/
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext); // Call next middleware
        }
        /*-------------------------------------------------------------------------*/
        catch (Exception ex)
        {
            if (ex.InnerException != null)
                _logger.LogError("{Type} {Message}",
                    ex.InnerException.GetType().ToString(),
                    ex.InnerException.Message);
            else
                _logger.LogError("{Type} {Message}",
                    ex.GetType().ToString(), ex.Message);
        /*-------------------------------------------------------------------------*/
            // Throw the Error in JSON response
            httpContext.Response.StatusCode = 500;
            // Set the response content type to JSON
            httpContext.Response.ContentType = "application/json";
            //Internal Server Error
            await httpContext.Response.WriteAsJsonAsync(
                new { Message = ex.Message, Type =
                ex.GetType().ToString()
                });
        }
    }

}

public static class ExceptionHandlingMiddlewareExtention
{
    ///
    ///<summary>
    ///this extenion method used by App
    ///<para name="Iwebappbuilder"></para>
    /// </summary>
    /// 
    public static IApplicationBuilder UseExcptionHandlingMiddleware(
        this IApplicationBuilder Builder
        )
    {
        return Builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}

