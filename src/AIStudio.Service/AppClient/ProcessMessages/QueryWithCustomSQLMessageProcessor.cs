using AIStudio.Service.AppClient.Models;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIStudio.Service.AppClient.ProcessMessages
{
    public class QueryWithCustomSQLMessageProcessor : AbstractProcessor
    {
        private readonly AppMessage _message;
        public QueryWithCustomSQLMessageProcessor(AppMessage message)
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
                string sql = _message.Datas[1];
                List<object> args = _message.Datas[2].ToList<object>();
                #endregion

                Type type = AbstractProcessor.Types[tableName];
                if (type == null)
                {
                    throw new ProcessorException(ResponseCode.SERVER_UNKNOWN_MESSAGE_TYPE, "没有该数据类型！");
                }

                string json = await QueryMessageProcessor.SubProcessData(tableName, null, sql, args);

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
    }
}
