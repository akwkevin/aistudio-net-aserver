using AIStudio.Service.AppClient.Models;
using ICSharpCode.SharpZipLib.GZip;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AIStudio.Service.AppClient.HttpClients
{
    /// <summary>
    /// 客户端获取Api
    /// </summary>
    public class HttpClientHelper
    {
        private static HttpClientHelper instance = null;
        private static object obj = new object();
        private IHttpClientFactory httpClientFactory;

        public static HttpClientHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        if (instance == null)
                        {
                            instance = new HttpClientHelper();
                            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
                            instance.httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                        }
                    }
                }
                return instance;
            }
        }


        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="json">发送的参数字符串，只能用json</param>
        /// <returns>返回的字符串</returns>
        public async Task<string> PostAsyncJson(string url, string json, Dictionary<string, string> header = null, CompressionType zip = CompressionType.None)
        {
            HttpClient client = httpClientFactory.CreateClient();
            HttpContent content = new StringContent(json);
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            switch (zip)
            {
                case CompressionType.None: break;
                case CompressionType.GZip: responseBody = CompressionHelper.DeCompress(responseBody, zip); break;
                case CompressionType.BZip2: responseBody = CompressionHelper.DeCompress(responseBody, zip); break;
                case CompressionType.Zip: responseBody = CompressionHelper.DeCompress(responseBody, zip); break;
            }

            return responseBody;
        }


        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="data">发送的参数字符串</param>
        /// <returns>返回的字符串</returns>
        public async Task<string> PostAsync(string url, HttpContent content, Dictionary<string, string> header = null, bool Gzip = false)
        {
            //HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false });
            HttpClient client = httpClientFactory.CreateClient();
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = "";
            if (Gzip)
            {
                GZipInputStream inputStream = new GZipInputStream(await response.Content.ReadAsStreamAsync());
                responseBody = new StreamReader(inputStream).ReadToEnd();
            }
            else
            {
                responseBody = await response.Content.ReadAsStringAsync();

            }

            return responseBody;
        }

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="data">发送的参数字符串</param>
        /// <returns>返回的字符串</returns>
        public async Task<string> PostAsync(string url, string data, Dictionary<string, string> header = null, bool Gzip = false)
        {
            //HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false });
            HttpClient client = httpClientFactory.CreateClient();
            HttpContent content = new StringContent(data);
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = "";
            if (Gzip)
            {
                GZipInputStream inputStream = new GZipInputStream(await response.Content.ReadAsStreamAsync());
                responseBody = new StreamReader(inputStream).ReadToEnd();
            }
            else
            {
                responseBody = await response.Content.ReadAsStringAsync();

            }

            return responseBody;
        }

        public async Task<byte[]> GetByteArrayAsync(string uri, Dictionary<string, string> header = null)
        {
            HttpClient client = httpClientFactory.CreateClient();
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }

            byte[] urlContents = await client.GetByteArrayAsync(uri);
            return urlContents;
        }

        /// <summary>
        /// 使用get方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <returns>返回的字符串</returns>
        public async Task<string> GetAsync(string url, Dictionary<string, string> header = null, bool Gzip = false)
        {

            HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false });
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();//用来抛异常的
            string responseBody = "";
            if (Gzip)
            {
                GZipInputStream inputStream = new GZipInputStream(await response.Content.ReadAsStreamAsync());
                responseBody = new StreamReader(inputStream).ReadToEnd();
            }
            else
            {
                responseBody = await response.Content.ReadAsStringAsync();

            }
            return responseBody;
        }

        /// <summary>
        /// 使用post返回异步请求直接返回对象
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <typeparam name="T2">请求对象类型</typeparam>
        /// <param name="url">请求链接</param>
        /// <param name="obj">请求对象数据</param>
        /// <returns>请求返回的目标对象</returns>
        public async Task<T> PostObjectAsync<T, T2>(string url, T2 obj)
        {
            String json = JsonConvert.SerializeObject(obj);
            string responseBody = await PostAsyncJson(url, json); //请求当前账户的信息
            return JsonConvert.DeserializeObject<T>(responseBody);//把收到的字符串序列化
        }

        /// <summary>
        /// 使用Get返回异步请求直接返回对象
        /// </summary>
        /// <typeparam name="T">请求对象类型</typeparam>
        /// <param name="url">请求链接</param>
        /// <returns>返回请求的对象</returns>
        public async Task<T> GetObjectAsync<T>(string url)
        {
            string responseBody = await GetAsync(url); //请求当前账户的信息
            return JsonConvert.DeserializeObject<T>(responseBody);//把收到的字符串序列化
        }

    }
}
