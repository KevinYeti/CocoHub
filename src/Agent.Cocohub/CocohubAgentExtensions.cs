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

            Console.WriteLine("Press EXIT to stop.");
            while (true)
            {
                var input = Console.ReadLine().Trim().ToLower();
                if (input == "exit")
                {
                    Agent.Cocohub.Startup.Stop();
                    break;
                }
            }

            return builder;
        }

    }
}
