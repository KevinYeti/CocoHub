using Agent.Cocohub.Entity;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agent.Cocohub
{
    public static class CocohubAgentExtensions
    {
        public static IApplicationBuilder UseCocohubAgent(this IApplicationBuilder builder, Action<IEnumerable<LogEntity>> write)
        {
            Startup.StartWatch(write);
            return builder;
        }

    }
}
