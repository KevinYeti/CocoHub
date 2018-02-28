using Logger.Cocohub;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Tracer.Cocohub.Service
{
    public static class CocohubServiceCollectionExtensions
    {
        public static void AddCocohub(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            LogStartup.Start();
        }
    }
}
