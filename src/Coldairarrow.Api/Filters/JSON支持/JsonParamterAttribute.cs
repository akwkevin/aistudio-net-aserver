using Coldairarrow.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coldairarrow.Api
{
    /// <summary>
    /// Json参数支持
    /// </summary>
    public class JsonParamterAttribute : Attribute, IActionFilter
    {
        /// <summary>
        /// Action执行之前执行
        /// </summary>
        /// <param name="context">过滤器上下文</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ContainsFilter<NoJsonParamterAttribute>())
                return;

            //参数映射：支持application/json
            string contentType = context.HttpContext.Request.ContentType;
            if (!contentType.IsNullOrEmpty() && contentType.Contains("application/json"))
            {
                var actionParameters = context.ActionDescriptor.Parameters;
                var allParamters = GetAllRequestParams(context.HttpContext);
                var actionArguments = context.ActionArguments;
                actionParameters.ForEach(aParamter =>
                {
                    string key = aParamter.Name;
                    if (allParamters.ContainsKey(key))
                    {
                        actionArguments[key] = allParamters[key]?.ToString()?.ChangeType_ByConvert(aParamter.ParameterType);
                    }
                    else
                    {
                        try
                        {
                            actionArguments[key] = allParamters.ToJson().ToObject(aParamter.ParameterType);
                        }
                        catch
                        {

                        }
                    }
                });
            }
        }

        /// <summary>
        /// Action执行完毕之后执行
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        /// <summary>
        /// 获取所有请求的参数（包括get参数和post参数）
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetAllRequestParams(HttpContext context)
        {
            Dictionary<string, object> allParams = new Dictionary<string, object>();

            var request = context.Request;
            List<string> paramKeys = new List<string>();
            var getParams = request.Query.Keys.ToList();
            var postParams = new List<string>();
            try
            {
                if (request.Method.ToLower() != "get")
                    postParams = request.Form.Keys.ToList();
            }
            catch
            {

            }
            paramKeys.AddRange(getParams);
            paramKeys.AddRange(postParams);

            paramKeys.ForEach(aParam =>
            {
                object value = null;
                if (request.Query.ContainsKey(aParam))
                    value = request.Query[aParam].ToString();
                else if (request.Form.ContainsKey(aParam))
                    value = request.Form[aParam].ToString();

                allParams.Add(aParam, value);
            });

            string contentType = request.ContentType?.ToLower() ?? "";

            //若为POST的application/json
            if (contentType.Contains("application/json"))
            {
                var stream = request.Body;
                string str = ReadToString(stream, Encoding.UTF8);
                if (!str.IsNullOrEmpty())
                {
                    var obj = str.ToJObject();
                    foreach (var aProperty in obj)
                    {
                        allParams[aProperty.Key] = aProperty.Value;
                    }
                }
            }
            return allParams;
        }

        /// <summary>
        /// 将流读为字符串
        /// 注：使用指定编码
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="encoding">指定编码</param>
        /// <returns></returns>
        public static string ReadToString(Stream stream, Encoding encoding)
        {
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            string resStr = string.Empty;
            resStr = new StreamReader(stream, encoding).ReadToEnd();

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            return resStr;
        }
    }
}