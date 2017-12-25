using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Logger.Cocohub;

namespace Microsoft.Extensions.DependencyInjection
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
