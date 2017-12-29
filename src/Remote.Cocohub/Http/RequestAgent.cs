using System;
using System.Net;
using System.Net.Http;
using Tracer.Cocohub.Context;
using TracerAttributes;

namespace Remote.Cocohub.Http
{
    public static class RequestAgent
    {
        public static void Get(Uri uri, Action<HttpStatusCode, string> callback)
        {
            try
            {
                //设置HttpClientHandler的AutomaticDecompression
                var handler = new HttpClientHandler();

                //创建HttpClient（注意传入HttpClientHandler）
                using (var http = new HttpClient(handler))
                {
                    if (TracerContext.Tracer != null)
                    {
                        TracerContext.Tracer.Enter();
                        http.DefaultRequestHeaders.Add("Cocohub-Tracer-Rpc", TracerContext.Tracer.ToString());
                    }

                    //等待回应
                    var response = http.GetAsync(uri).Result;

                    var code = response.StatusCode;
                    var content = response.Content.ReadAsStringAsync().Result;

                    TracerContext.Tracer.Leave();
                    //
                    callback(code, content);
                }
            }
            catch (Exception ex)
            {
                callback(HttpStatusCode.BadGateway, ex.Message);
            }
        }

        public static async void GetAsync(Uri uri, Action<HttpStatusCode, string> callback)
        {
            try
            {
                //设置HttpClientHandler的AutomaticDecompression
                var handler = new HttpClientHandler();

                //创建HttpClient（注意传入HttpClientHandler）
                using (var http = new HttpClient(handler))
                {
                    if (TracerContext.Tracer != null)
                    {
                        http.DefaultRequestHeaders.Add("Cocohub-Tracer-Rpc", TracerContext.Tracer.EnterAsync());
                    }

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
                callback(HttpStatusCode.BadGateway, ex.Message);
            }
        }

        public static void Post(Uri uri, HttpContent body, Action<HttpStatusCode, string> callback)
        {
            try
            {
                //设置HttpClientHandler的AutomaticDecompression
                var handler = new HttpClientHandler();

                //创建HttpClient（注意传入HttpClientHandler）
                using (var http = new HttpClient(handler))
                {
                    if (TracerContext.Tracer != null)
                    {
                        TracerContext.Tracer.Enter();
                        http.DefaultRequestHeaders.Add("Cocohub-Tracer-Rpc", TracerContext.Tracer.ToString());
                    }

                    //等待回应
                    var response = http.PostAsync(uri, body).Result;

                    var code = response.StatusCode;
                    var content = response.Content.ReadAsStringAsync().Result;

                    TracerContext.Tracer.Leave();
                    //
                    callback(code, content);
                }
            }
            catch (Exception ex)
            {
                callback(HttpStatusCode.BadGateway, ex.Message);
            }
        }

        public static async void PostAsync(Uri uri, HttpContent body, Action<HttpStatusCode, string> callback)
        {
            try
            {
                //设置HttpClientHandler的AutomaticDecompression
                var handler = new HttpClientHandler();

                //创建HttpClient（注意传入HttpClientHandler）
                using (var http = new HttpClient(handler))
                {
                    if (TracerContext.Tracer != null)
                    {
                        http.DefaultRequestHeaders.Add("Cocohub-Tracer-Rpc", TracerContext.Tracer.EnterAsync());
                    }

                    //await异步等待回应
                    var response = await http.PostAsync(uri, body);

                    var code = response.StatusCode;
                    var content = await response.Content.ReadAsStringAsync();
                    //
                    callback(code, content);
                }
            }
            catch (Exception ex)
            {
                callback(HttpStatusCode.BadGateway, ex.Message);
            }
        }

        public static void Delete(Uri uri, Action<HttpStatusCode, string> callback)
        {
            try
            {
                //设置HttpClientHandler的AutomaticDecompression
                var handler = new HttpClientHandler();

                //创建HttpClient（注意传入HttpClientHandler）
                using (var http = new HttpClient(handler))
                {
                    if (TracerContext.Tracer != null)
                    {
                        TracerContext.Tracer.Enter();
                        http.DefaultRequestHeaders.Add("Cocohub-Tracer-Rpc", TracerContext.Tracer.ToString());
                    }


                    //等待回应
                    var response = http.DeleteAsync(uri).Result;

                    var code = response.StatusCode;
                    var content = response.Content.ReadAsStringAsync().Result;

                    TracerContext.Tracer.Leave();
                    //
                    callback(code, content);
                }
            }
            catch (Exception ex)
            {
                callback(HttpStatusCode.BadGateway, ex.Message);
            }
        }

        public static async void DeleteAsync(Uri uri, Action<HttpStatusCode, string> callback)
        {
            try
            {
                //设置HttpClientHandler的AutomaticDecompression
                var handler = new HttpClientHandler();

                //创建HttpClient（注意传入HttpClientHandler）
                using (var http = new HttpClient(handler))
                {
                    if (TracerContext.Tracer != null)
                    {
                        http.DefaultRequestHeaders.Add("Cocohub-Tracer-Rpc", TracerContext.Tracer.EnterAsync());
                    }

                    //await异步等待回应
                    var response = await http.DeleteAsync(uri);

                    var code = response.StatusCode;
                    var content = await response.Content.ReadAsStringAsync();
                    //
                    callback(code, content);
                }
            }
            catch (Exception ex)
            {
                callback(HttpStatusCode.BadGateway, ex.Message);
            }
        }

        public static void Put(Uri uri, HttpContent body, Action<HttpStatusCode, string> callback)
        {
            try
            {
                //设置HttpClientHandler的AutomaticDecompression
                var handler = new HttpClientHandler();

                //创建HttpClient（注意传入HttpClientHandler）
                using (var http = new HttpClient(handler))
                {
                    if (TracerContext.Tracer != null)
                    {
                        TracerContext.Tracer.Enter();
                        http.DefaultRequestHeaders.Add("Cocohub-Tracer-Rpc", TracerContext.Tracer.ToString());
                    }


                    //等待回应
                    var response = http.PutAsync(uri, body).Result;

                    var code = response.StatusCode;
                    var content = response.Content.ReadAsStringAsync().Result;

                    TracerContext.Tracer.Leave();
                    //
                    callback(code, content);
                }
            }
            catch (Exception ex)
            {
                callback(HttpStatusCode.BadGateway, ex.Message);
            }
        }

        public static async void PutAsync(Uri uri, HttpContent body, Action<HttpStatusCode, string> callback)
        {
            try
            {
                //设置HttpClientHandler的AutomaticDecompression
                var handler = new HttpClientHandler();

                //创建HttpClient（注意传入HttpClientHandler）
                using (var http = new HttpClient(handler))
                {
                    if (TracerContext.Tracer != null)
                    {
                        http.DefaultRequestHeaders.Add("Cocohub-Tracer-Rpc", TracerContext.Tracer.EnterAsync());
                    }

                    //await异步等待回应
                    var response = await http.PutAsync(uri, body);

                    var code = response.StatusCode;
                    var content = await response.Content.ReadAsStringAsync();
                    //
                    callback(code, content);
                }
            }
            catch (Exception ex)
            {
                callback(HttpStatusCode.BadGateway, ex.Message);
            }
        }

    }
}
