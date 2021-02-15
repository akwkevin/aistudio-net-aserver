using AIStudio.Service.AppClient.HttpClients;
using AIStudio.Service.AppClient.Models;
using Coldairarrow.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AIStudio.Service.AppClient
{
    public class NetworkTransfer
    {
        public string Url { get; set; }
        public IAppHeader Header { get; set; }

        public NetworkTransfer(string url, IAppHeader header, TimeSpan timeout)
        {
            Url = url;
            Header = header;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public async Task<AjaxResult<string>> GetToken()
        {
            if (Header is AppTokenHeader)
            {
                AppTokenHeader header = Header as AppTokenHeader;

                var content = await HttpClientHelper.Instance.PostAsync((string.Format("{0}/Base_Manage/Home/SubmitLogin?userName={1}&password={2}", Url, header.UserName, header.Password)), content: null);
                var result = JsonConvert.DeserializeObject<AjaxResult<string>>(content);
                header.Token = result.Data;
                return result;
            }

            return null;
        }

        public async Task<AjaxResult<string>> GetData(string url)
        {
            var content = await HttpClientHelper.Instance.PostAsync(url, content: null, Header.SetHeader());
            var result = JsonConvert.DeserializeObject<AjaxResult<string>>(content);
            return result;
        }

        public async Task<AjaxResult<string>> PostData(AppMessage apiMessage)
        {
            string json = JsonConvert.SerializeObject(apiMessage);
            var content = await HttpClientHelper.Instance.PostAsyncJson(string.Format("{0}/api/AppServer/ProcessMessage", Url), json, Header.SetHeader(), apiMessage.Zip);
            var result = JsonConvert.DeserializeObject<AjaxResult<string>>(content);
            return result;
        }

        public async Task<AjaxResult<string>> Query(string tableName, ICollection<string> columns, string condition, object[] args, CompressionType zip)
        {
            AppMessage apiMessage = new AppMessage();
            apiMessage.Type = WebMessageType.QueryRequest;
            apiMessage.Zip = zip;
            apiMessage.Datas = new string[]
            {
                tableName,
                StandardTimeFormatJsonConvertor.SerializeObject(columns),
                condition,
                StandardTimeFormatJsonConvertor.SerializeObject(args)
            };

            return await PostData(apiMessage);
        }

        public async Task<AjaxResult<string>> Add(string tableName, string datajson, ICollection<string> columns, CompressionType zip)
        {
            AppMessage apiMessage = new AppMessage();
            apiMessage.Type = WebMessageType.AddRequest;
            apiMessage.Zip = zip;
            apiMessage.Datas = new string[]
            {
                tableName,
                datajson,
                StandardTimeFormatJsonConvertor.SerializeObject(columns),
            };

            return await PostData(apiMessage);
        }

        public async Task<AjaxResult<string>> Modify(string tableName, ICollection<string> columns, string datajson, CompressionType zip)
        {
            AppMessage apiMessage = new AppMessage();
            apiMessage.Type = WebMessageType.ModifyRequest;
            apiMessage.Zip = zip;
            apiMessage.Datas = new string[]
            {
                tableName,
                StandardTimeFormatJsonConvertor.SerializeObject(columns),
                datajson,
            };

            return await PostData(apiMessage); ;
        }

        public async Task<AjaxResult<string>> Delete(string tableName, string primaryKeyColumn, ICollection<object> ids, CompressionType zip)
        {
            AppMessage apiMessage = new AppMessage();
            apiMessage.Type = WebMessageType.DeleteRequest;
            apiMessage.Zip = zip;
            apiMessage.Datas = new string[]
            {
                tableName,
                primaryKeyColumn,
                StandardTimeFormatJsonConvertor.SerializeObject(ids),
            };

            return await PostData(apiMessage);
        }

        public async Task<AjaxResult<string>> ComplexOperation(string addJson, string modifyJson, string deleteJson, CompressionType zip)
        {
            AppMessage apiMessage = new AppMessage();
            apiMessage.Type = WebMessageType.ComplexOperationRequest;
            apiMessage.Zip = zip;
            apiMessage.Datas = new string[]
            {
                addJson,
                modifyJson,
                deleteJson,
            };

            return await PostData(apiMessage);
        }

        public async Task<AjaxResult<string>> ComplexQuery(ICollection<ComplexQuery> queries, CompressionType zip)
        {
            AppMessage apiMessage = new AppMessage();
            apiMessage.Type = WebMessageType.ComplexQueryRequest;
            apiMessage.Zip = zip;
            apiMessage.Datas = new string[]
            {
                StandardTimeFormatJsonConvertor.SerializeObject(queries)
            };

            return await PostData(apiMessage);
        }

        public async Task<AjaxResult<string>> QueryWithCustomSQL(string tableName, string sql, object[] args, CompressionType zip)
        {
            AppMessage apiMessage = new AppMessage();
            apiMessage.Type = WebMessageType.QueryWithCustomSQLRequest;
            apiMessage.Zip = zip;
            apiMessage.Datas = new string[]
            {
                tableName,
                sql,
                StandardTimeFormatJsonConvertor.SerializeObject(args),
            };

            return await PostData(apiMessage);
        }

        public async Task<AjaxResult<string>> UploadFile(string path, string fileName, string qq)
        {
            var data = new MultipartFormDataContent();
            //添加字符串参数，参数名为qq
            data.Add(new StringContent(qq), "qq");

            //添加文件参数，参数名为files，文件名为123.png
            data.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(path)), "file", fileName);

            var content = await HttpClientHelper.Instance.PostAsync(string.Format("{0}/api/FileServer/SaveFile", Url), data, Header.SetHeader());
            var result = JsonConvert.DeserializeObject<AjaxResult<string>>(content);
            return result;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="savepath"></param>
        /// <returns></returns>
        public async Task<AjaxResult<string>> DownLoadFile(string fullpath, string savepath)
        {
            FileStream fs = null;
            try
            {
                var content = await HttpClientHelper.Instance.GetByteArrayAsync(fullpath);
                fs = new FileStream(savepath, FileMode.Create);
                fs.Write(content, 0, content.Length);
                return new AjaxResult<string>() { Success = true };
            }
            catch (Exception ex)
            {
                return new AjaxResult<string>() { Success = false, Msg = ex.ToString() };
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
    }
}
