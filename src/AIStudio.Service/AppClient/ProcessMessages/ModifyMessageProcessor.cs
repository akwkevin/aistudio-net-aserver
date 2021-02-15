﻿using AIStudio.Service.AppClient.Models;
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
    public class ModifyMessageProcessor : AbstractProcessor
    {
        private readonly AppMessage _message;
        public ModifyMessageProcessor(AppMessage message)
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
                Type type = AbstractProcessor.Types[tableName];
                if (type == null)
                {
                    throw new ProcessorException(ResponseCode.SERVER_UNKNOWN_MESSAGE_TYPE, "没有该数据类型！");
                }

                List<string> columns = _message.Datas[1].ToList<string>();

                IList objs = JsonConvert.DeserializeObject(_message.Datas[2], typeof(List<>).MakeGenericType(new Type[]
                {
                    type
                })) as IList;
                #endregion

                await ProcessData(tableName, objs, columns);

                res.Success = true;
                res.Data = null;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.ErrorCode = (int)ResponseCode.SERVER_EXCEPTION;
                res.Msg = ex.Message;
            }

            return res;
        }

        public static async Task<string> ProcessData(string tableName, IList objs, IList<string> columns)
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
            MethodInfo updateListAsync = business.GetType().GetMethod("UpdateAnyAsync", new Type[] { objs.GetType(), columns.GetType()});
            Task task = updateListAsync.Invoke(business, new Object[] { objs, columns }) as Task;
            await task;
            object list = task.GetType().GetProperty("Result").GetValue(task, null); //result就是异步
            #endregion

            return StandardTimeFormatJsonConvertor.SerializeObject(list);
        }
    }
}
