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
                    //by default: the span should enter one level when a remote call is happened.
                    //if (tracer.Contains(":"))
                    //{
                    //    var splits = tracer.Split(TracerIndentity.Spliter, StringSplitOptions.RemoveEmptyEntries);
                    //    string spanId = splits[1];
                    //    if (spanId.Contains("."))
                    //    {
                    //        string broFuncSpanNum = spanId.Substring(spanId.LastIndexOf(".") + 1);
                    //        int cntFuncSpanNum = Convert.ToInt32(broFuncSpanNum) + 1;
                    //        spanId = spanId.Substring(0, spanId.LastIndexOf(".") + 1) + cntFuncSpanNum.ToString();
                    //    }
                    //    tracer = splits[0] + TracerIndentity.Spliter[0] + spanId;
                    //}

                    context.Items.Add(TracerContext._tracer, TracerIndentity.FromString(tracer));
                }
                else
                {
                    context.Items.Add(TracerContext._tracer, TracerIndentity.Create()); 
                }
            }

            //add tracerId to response header when return
            if (!context.Response.Headers.ContainsKey(TracerContext._tracer) 
                && TracerIndentity.Current != null)
            {
                context.Response.Headers.Add(TracerContext._tracer, TracerIndentity.Current.ToString());
            }

            // Call the next delegate/middleware in the pipeline
            return this._next(context);
        }
    }
}