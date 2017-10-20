using Microsoft.AspNetCore.Http;

namespace Tracer.Cocohub.Context
{
    public static class TracerContext
    {
        internal static string _tracer = "Cocohub-Tracer";

        private static IHttpContextAccessor _contextAccessor;

        public static HttpContext CurrentHttpContext
        {
            get
            {
                if (_contextAccessor == null)
                    return null;
                else
                    return _contextAccessor.HttpContext;
            }
        }

        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
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
            else if (!http.Items.ContainsKey(_tracer))
            {
                //理论上此处代码应该走不进来，因为middleware中已经做过此事
                //when _tracer is in header but not in context 
                if (http.Request.Headers.Keys.Contains(_tracer))
                {
                    http.Items.Add(_tracer, TracerIndentity.Create(http.Request.Headers[_tracer].ToString()));
                }
                else
                {
                    http.Items.Add(_tracer, TracerIndentity.Create());
                }
            }

            var tracer = http.Items[_tracer] as TracerIndentity;
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
