using System;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;
using Tracer.Cocohub.Context;

namespace Tracer.Cocohub.Middleware
{
    public class CocohubMiddleware
    {
        private readonly RequestDelegate _next;

        public CocohubMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            //init tracer in context 
            if (!context.Items.ContainsKey(TracerContext._tracer))
            {
                //when _tracer is in header but not in context 
                if (context.Request.Headers.Keys.Contains(TracerContext._tracer))
                {
                    var tracer = context.Request.Headers[TracerContext._tracer].ToString();
                    context.Items.Add(TracerContext._tracer, TracerIndentity.Create(tracer));
                }
                else
                {
                    context.Items.Add(TracerContext._tracer, TracerIndentity.Create()); 
                }
            }

            // Call the next delegate/middleware in the pipeline
            return this._next(context);
        }
    }
}