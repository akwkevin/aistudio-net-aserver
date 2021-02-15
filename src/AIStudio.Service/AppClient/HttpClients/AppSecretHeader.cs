using Coldairarrow.Util;
using System;
using System.Collections.Generic;

namespace AIStudio.Service.AppClient.HttpClients
{
    /// <summary>
    /// 密匙App
    /// </summary>
    public class AppSecretHeader: IAppHeader
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string Body { get; set; } = "";

        /// <summary>
        /// 根据AppId与appSecret来访问服务
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        public AppSecretHeader(string appId, string appSecret)
        {
            AppId = appId;
            AppSecret = appSecret;
        }

        /// <summary>
        /// 设置Header信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> SetHeader()
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string guid = Guid.NewGuid().ToString();
            header.Add("appId", AppId);
            header.Add("time", time);
            header.Add("guid", guid);
            string sign = $"{AppId}{time}{guid}{Body}{AppSecret}".ToMD5String();
            header.Add("sign", sign);

            return header;
        }
    }
}
