using Microsoft.AspNetCore.Builder;
using AgriculturalLandManagement.Middleware;
namespace AgriculturalLandManagement.Helper;
public static class RequestTimingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestTiming(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestTimingMiddleware>();
    }
}
