using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Tracer.Cocohub.Context;

namespace Tracer.Cocohub.Middleware
{
    public static class CocohubMiddlewareExtensions
    {
        public static IApplicationBuilder UseCocohub(this IApplicationBuilder builder)
        {
            var httpContextAccessor = builder.ApplicationServices.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            TracerContext.Configure(httpContextAccessor);
            return builder.UseMiddleware<CocohubMiddleware>();
        }
    }
}
