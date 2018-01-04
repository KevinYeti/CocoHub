using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Tracer.Cocohub;
using Tracer.Cocohub.Context;


namespace Remote.Cocohub.Http
{
    public static class RequestAgent
    {
        private static string _textEnterWithTracer = "{{\"Action\":\"Enter\",\"Method\":\"{0}\",\"Params\":\"{1}\",\"TracerId\":\"{2}\",\"SpanId\":\"{3}\"}}";
        private static string _textReturnWithTracer = "{{\"Action\":\"Return\",\"Method\":\"{0}\",\"Result\":\"{1}\",\"Time\":{2},\"TracerId\":\"{3}\",\"SpanId\":\"{4}\"}}";
        private static string _exception = "$exception={0}";
        private static HttpClient _httpClient = new HttpClient();


        public static void Send(HttpMethod method, Uri uri, HttpContent body, Action<HttpStatusCode, string> callback)
        {
            bool hasEntered = false;
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                using (var request = new HttpRequestMessage(method, uri))
                {
                    if (body != null)
                    {
                        request.Content = body;
                    }

                    if (TracerContext.Tracer != null)
                    {
                        TracerContext.Tracer.Enter();
                        hasEntered = true;

                        request.Headers.Add("Cocohub-Tracer-Rpc", TracerContext.Tracer.ToString());
                    }
                    using (var response = _httpClient.SendAsync(request).Result)
                    {
                        var code = response.StatusCode;
                        var content = response.Content.ReadAsStringAsync().Result;

                        TracerContext.Tracer.Leave();
                        //
                        callback?.Invoke(code, content);
                    }
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                string logEx = string.Format(_exception, ex.Message + ex.StackTrace);
                string logEnter = string.Format(_textEnterWithTracer, "Http" + method.Method, uri.ToString(), TracerContext.Tracer.TracerId, TracerContext.Tracer.SpanId);
                string logReturn = string.Format(_textReturnWithTracer, "Http" + method.Method, logEx, sw.ElapsedMilliseconds, TracerContext.Tracer.TracerId, TracerContext.Tracer.SpanId);

                if (hasEntered)
                {
                    TracerContext.Tracer.Leave();
                }

                //write log
                Log.Error(logEnter);
                Log.Error(logReturn);

                callback(HttpStatusCode.BadGateway, ex.Message);
            }
            finally
            {
                if (sw.IsRunning)
                {
                    sw.Stop();
                }
            }
        }

        public static async void SendAsync(HttpMethod method, Uri uri, HttpContent body, Action<HttpStatusCode, string> callback)
        {
            var sw = new Stopwatch();
            sw.Start();
            var tracer = TracerContext.Tracer.EnterAsync();

            try
            {
                using (var request = new HttpRequestMessage(method, uri))
                {
                    if (body != null)
                    {
                        request.Content = body;
                    }

                    if (TracerContext.Tracer != null)
                    {
                        request.Headers.Add("Cocohub-Tracer-Rpc", tracer);
                    }
                    using (var response = await _httpClient.SendAsync(request))
                    {
                        var code = response.StatusCode;
                        var content = await response.Content.ReadAsStringAsync();

                        //
                        callback?.Invoke(code, content);
                    }
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                var tracers = tracer.Split(TracerIndentity.Spliter, StringSplitOptions.RemoveEmptyEntries);
                string logEx = string.Format(_exception, ex.Message + ex.StackTrace);
                string logEnter = string.Format(_textEnterWithTracer, "Http" + method.Method, uri.ToString(), tracers[0], tracers[1]);
                string logReturn = string.Format(_textReturnWithTracer, "Http" + method.Method, logEx, sw.ElapsedMilliseconds, tracers[0], tracers[1]);

                //write log
                Log.Error(logEnter);
                Log.Error(logReturn);

                callback(HttpStatusCode.BadGateway, ex.Message);
            }
            finally
            {
                if (sw.IsRunning)
                {
                    sw.Stop();
                }
            }
        }
    }
}
