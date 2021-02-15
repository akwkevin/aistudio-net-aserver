using AIStudio.Service.AppClient.Models;
using Coldairarrow.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIStudio.Service.AppClient.ProcessMessages
{
    /// <summary>
    /// 
    /// </summary>
    public class ComplexQueryMessageProcessor : AbstractProcessor
    {
        private readonly AppMessage _message;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ComplexQueryMessageProcessor(AppMessage message)
        {
            _message = message;
        }

        public override async Task<AjaxResult<string>> DoResponse()
        {
            AjaxResult<string> res = new AjaxResult<string>();

            try
            {
                #region 参数
                ComplexQuery[] querys = JsonConvert.DeserializeObject<ComplexQuery[]>(_message.Datas[0]);
                #endregion

                List<string> tableDatas = new List<string>();
                List<string> tableNames = new List<string>();

                foreach (ComplexQuery query in querys)
                {
                    var json = await QueryMessageProcessor.ProcessData(query.TableName, query.Columns, query.Condition, query.Args);
                    tableNames.Add(query.TableName);
                    tableDatas.Add(json);
                }

                ComplexQueryResult result = new ComplexQueryResult(tableNames.ToArray(), tableDatas.ToArray());

                res.Success = true;
                res.Data = StandardTimeFormatJsonConvertor.SerializeObject(result);
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
