using System;
using System.Net;
using System.Net.Http;
using Tracer.Cocohub.Context;
using TracerAttributes;

namespace Remote.Cocohub.Http
{
    public static class RequestAgent
    {
        public static async void Get(Uri uri, Action<HttpStatusCode, string> callback)
        {
            try
            {
                //设置HttpClientHandler的AutomaticDecompression
                var handler = new HttpClientHandler();

                //创建HttpClient（注意传入HttpClientHandler）
                using (var http = new HttpClient(handler))
                {
                    if (TracerContext.Tracer != null)
                        http.DefaultRequestHeaders.Add("Cocohub-Tracer", TracerContext.Tracer.ToString());


                    //await异步等待回应
                    var response = await http.GetAsync(uri);

                    var code = response.StatusCode;
                    var content = await response.Content.ReadAsStringAsync();
                    //
                    callback(code, content);
                }
            }
            catch (Exception ex)
            {
                callback(HttpStatusCode.NotFound, ex.Message);
            }
        }



    }
}
