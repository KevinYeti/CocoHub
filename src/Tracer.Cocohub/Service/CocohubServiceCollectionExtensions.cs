using Logger.Cocohub;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Tracer.Cocohub.Service
{
    public static class CocohubServiceCollectionExtensions
    {
        public static void AddCocohub(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            LogStartup.Start();
        }
    }
}
