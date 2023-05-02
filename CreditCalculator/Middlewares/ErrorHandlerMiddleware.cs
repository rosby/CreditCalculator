using CreditCalculator.Exceptions;
using Newtonsoft.Json;

namespace CreditCalculator.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await ReturnAsJson(context, new ExceptionResponseInfo {Code = 400, Message = ex.Message});
        }
    }

    private async Task ReturnAsJson(HttpContext context, object obj)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(Json(obj));
    }
    
    private static string Json(object data)
    {
        return JsonConvert.SerializeObject(data);
    }
}

public class ExceptionResponseInfo
{
    public int Code { get; set; }
    
    public string Message { get; set; }
}