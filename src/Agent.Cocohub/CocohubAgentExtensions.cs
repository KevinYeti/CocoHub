using Agent.Cocohub.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agent.Cocohub
{
    public static class CocohubAgentExtensions
    {
        public static IApplicationLifetime UseCocohubAgent(this IApplicationLifetime lifetime, Action<IEnumerable<LogEntity>> write)
        {
            Startup.StartWatch(write);

            lifetime.ApplicationStopping.Register(onShutdown);

            return lifetime;
        }

        private static void onShutdown()
        {
            Agent.Cocohub.Startup.Stop();
        }
    }
}
