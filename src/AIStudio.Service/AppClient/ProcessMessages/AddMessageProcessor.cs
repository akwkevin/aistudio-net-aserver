using AIStudio.Service.AppClient.Models;
using Coldairarrow.Util;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AIStudio.Service.AppClient.ProcessMessages
{
    /// <summary>
    /// 
    /// </summary>
    public class AddMessageProcessor : AbstractProcessor
    {
        private readonly AppMessage _message;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public AddMessageProcessor(AppMessage message)
        {
            _message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task<AjaxResult<string>> DoResponse()
        {
            AjaxResult<string> res = new AjaxResult<string>();

            try
            {
                #region 参数
                string tableName = _message.Datas[0];
                Type type = AbstractProcessor.Types[tableName];
                if (type == null)
                {
                    throw new ProcessorException(ResponseCode.SERVER_UNKNOWN_MESSAGE_TYPE, "没有该数据类型！");
                }

                IList objs = JsonConvert.DeserializeObject(_message.Datas[1], typeof(List<>).MakeGenericType(new Type[]
                {
                    type
                })) as IList;

                List<string> columns = _message.Datas[2].ToList<string>();
                #endregion

                var json = await ProcessData(tableName, objs, columns);

                res.Success = true;              
                res.Data = json;
            }
            catch (ProcessorException ex)
            {
                res.Success = false;
                res.ErrorCode = (int)ex.GetResponseCode();
                res.Msg = ex.GetError();
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.ErrorCode = (int)ResponseCode.SERVER_EXCEPTION;
                res.Msg = ex.Message;
            }           

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="objs"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static async Task<string> ProcessData(string tableName, IList objs, ICollection<string> columns)
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
            MethodInfo insertListAsync = business.GetType().GetMethod("InsertBackAsync", new Type[] { objs.GetType() });
            Task task = insertListAsync.Invoke(business, new Object[] { objs }) as Task;
            await task;
            object list = task.GetType().GetProperty("Result").GetValue(task, null); //result就是异步
            #endregion

            var json = columns == null || columns.Count == 0 ? null : StandardTimeFormatJsonConvertor.SerializeObject(list, columns);
            return json;
        }
    }
}
