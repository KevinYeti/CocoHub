﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;

namespace Tracer.Cocohub.Context
{
    public static class TracerContext
    {
        internal static string _tracer = "Cocohub-Tracer";
        internal static string _tracerRpc = "Cocohub-Tracer-Rpc";

        private static IHttpContextAccessor _contextAccessor;

        public static HttpContext CurrentHttpContext {
            get {
                if (_contextAccessor == null)
                {
                    System.Diagnostics.Debug.WriteLine("Warning: CurrentHttpContext is null, this maybe cased by context accessor is null.");
                    return null;
                }
                    
                else
                    return _contextAccessor.HttpContext;
            }
        }

        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public static string IP {
            get {
                string _ip = "0.0.0.0";
                if (_contextAccessor != null && _contextAccessor.HttpContext != null)
                {
                    string ip = string.Empty;
                    //NLB
                    if (_contextAccessor.HttpContext.Request != null
                        && _contextAccessor.HttpContext.Request.Headers != null
                        && _contextAccessor.HttpContext.Request.Headers.Count > 0)
                    {
                        ip = _contextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                    }
                    //normal network
                    if (string.IsNullOrEmpty(ip) 
                        && _contextAccessor.HttpContext.Connection != null
                        && _contextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                    {
                        ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                    }

                    if (!string.IsNullOrEmpty(ip))
                        _ip = ip;
                }

                return _ip;
            }
        }

        public static TracerIndentity Tracer
        {
            get
            {
                var http = CurrentHttpContext;
                if (http == null || http.Items[_tracer] == null)
                    return null;
                else
                    return http.Items[_tracer] as TracerIndentity;
            }
        }

        public static void Enter()
        {
            var http = CurrentHttpContext;
            if (http == null)
            {
                return;
            }
            else if (!http.Items.ContainsKey(_tracer) && http.Items.ContainsKey(_tracerRpc))
            {
                //在此处转换带rpc标记的tracer为普通tracer,并且不需要再次Enter,因为在调用前已经做过TryRpcEnter
                http.Items.Add(_tracer, http.Items[_tracerRpc]);
                http.Items.Remove(_tracerRpc);
                return;
            }

            var tracer = http.Items[_tracer] as TracerIndentity;
            if (tracer == null)
                return;
            else
                tracer.Enter();
        }

        public static void Leave()
        {
            var http = CurrentHttpContext;
            if (http == null)
            {
                return;
            }
            else if (http.Items.ContainsKey(_tracer))
            {
                var tracer = http.Items[_tracer] as TracerIndentity;
                tracer.Leave();
            }

        }
    }
}
