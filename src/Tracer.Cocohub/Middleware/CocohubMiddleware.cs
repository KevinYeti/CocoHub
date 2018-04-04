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
            try
            {
                TracerIndentity _tracer = null;
                //init tracer in context 
                if (!context.Items.ContainsKey(TracerContext._tracer)
                    && !context.Items.ContainsKey(TracerContext._tracerRpc))
                {
                    //when _tracer is in header but not in context 
                    if (context.Request.Headers.Keys.Contains(TracerContext._tracer))
                    {
                        var tracer = context.Request.Headers[TracerContext._tracer].ToString();
                        _tracer = TracerIndentity.FromString(tracer);
                        context.Items.Add(TracerContext._tracer, _tracer);
                    }
                    else if (context.Request.Headers.Keys.Contains(TracerContext._tracerRpc))
                    {
                        //request from RPC 
                        var tracerRpc = context.Request.Headers[TracerContext._tracerRpc].ToString();
                        _tracer = TracerIndentity.FromString(tracerRpc);
                        context.Items.Add(TracerContext._tracerRpc, _tracer);
                    }
                    else
                    {
                        //new request, new context 
                        _tracer = TracerIndentity.Create();
                        context.Items.Add(TracerContext._tracer, _tracer);
                    }
                }

                //add tracerId to response header for return
                if (!context.Response.Headers.ContainsKey(TracerContext._tracer)
                    && _tracer != null)
                {
                    context.Response.Headers.Add(TracerContext._tracer, _tracer.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            // Call the next delegate/middleware in the pipeline
            return this._next(context);
        }
    }
}