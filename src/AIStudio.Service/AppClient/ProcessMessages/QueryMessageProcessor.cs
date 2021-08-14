using AIStudio.Service.AppClient.Models;
using Coldairarrow.Util;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AIStudio.Service.AppClient.ProcessMessages
{
    public class QueryMessageProcessor : AbstractProcessor
    {
        private readonly AppMessage _message;
        public QueryMessageProcessor(AppMessage message)
        {
            _message = message;
        }

        public override async Task<AjaxResult<string>> DoResponse()
        {
            AjaxResult<string> res = new AjaxResult<string>();

            try
            {
                #region 参数
                string tableName = _message.Datas[0];
                List<string> columns = _message.Datas[1].ToList<string>();
                string condition = _message.Datas[2];
                List<object> args = _message.Datas[3].ToList<object>();
                #endregion

                Type type = AbstractProcessor.Types[tableName];
                if (type == null)
                {
                    throw new ProcessorException(ResponseCode.SERVER_UNKNOWN_MESSAGE_TYPE, "没有该数据类型！");
                }

                string json = await ProcessData(tableName, columns, condition, args);

                res.Success = true;
                res.Data = json;
            }
            catch(Exception ex)
            {
                res.Success = false;
                res.ErrorCode = (int)ResponseCode.SERVER_EXCEPTION;
                res.Msg = ex.Message;
            }           

            return res;
        }

        public static async Task<string> ProcessData(string tableName, IList<string> columns, string condition, IList<object> args)
        {
            #region 拼接字符串
            StringBuilder builder = new StringBuilder();
            //bool flag = columns != null && columns.Count != 0;
            //if (flag)
            //{
            //    builder.Append("select ");
            //    builder.Append(string.Join(",", columns));
            //    builder.Append(" from ");
            //}
            //else
            {
                builder.Append("select * from ");
            }
            builder.Append(tableName);
            builder.Append(" where ");
            bool isSelectAll = string.IsNullOrWhiteSpace(condition) || args == null || args.Count == 0;
            builder.Append(isSelectAll ? "1 = 1" : condition);
            #endregion

            var json = await SubProcessData(tableName, columns, builder.ToString(), args);
            
            return json;
        }

        public static async Task<string> SubProcessData(string tableName, IList<string> columns, string sql, IList<object> args)
        {
            #region 获取对应程序集及服务
            var businesstype = AbstractProcessor.IBusinessTypes["I" + tableName + "Business"];
            if (businesstype == null)
            {
                throw new ProcessorException(ResponseCode.SERVER_UNKNOWN_MESSAGE_TYPE, "没有该数据类型的处理方法！");
            }

            var business = ServiceLocator.Instance.GetRequiredService(businesstype);
            if (business == null)
            {
                throw new ProcessorException(ResponseCode.SERVER_UNKNOWN_MESSAGE_TYPE, "该数据类型的处理方法没有注册！");
            }
            #endregion

            #region 反射获取数据方法
            List<ValueTuple<string, object>> paramters = new List<(string, object)>();
            if (args != null)
            {
                var paramterNames = Regex.Matches(sql, @"@\S+");
                if (args.Count == paramterNames.Count)
                {
                    for (int i = 0; i < paramterNames.Count; i++)
                    {
                        paramters.Add((paramterNames[i].Value, args[i]));
                    }
                }
            }
            MethodInfo getAllBySql = business.GetType().GetMethod("GetListBySqlAsync");
            Task task = getAllBySql.Invoke(business, new Object[] { sql, paramters.ToArray() }) as Task;
            await task;
            object list = task.GetType().GetProperty("Result").GetValue(task, null); //result就是异步
            #endregion

            var json = StandardTimeFormatJsonConvertor.SerializeObject(list, columns);

            return json;
        }

    }
}
