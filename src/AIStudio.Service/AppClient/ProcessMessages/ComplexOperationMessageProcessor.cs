using AIStudio.Service.AppClient.Models;
using Coldairarrow.Util;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIStudio.Service.AppClient.ProcessMessages
{
    /// <summary>
    /// 
    /// </summary>
    public class ComplexOperationMessageProcessor : AbstractProcessor
    {
        private readonly AppMessage _message;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ComplexOperationMessageProcessor(AppMessage message)
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

                IDictionary<string, Tuple<ICollection<string>, ICollection<object>>> addObjs = JsonConvert.DeserializeObject<IDictionary<string, Tuple<ICollection<string>, ICollection<object>>>>(_message.Datas[0]);
                IDictionary<string, Tuple<ICollection<string>, ICollection<object>>> updateObjs = JsonConvert.DeserializeObject<IDictionary<string, Tuple<ICollection<string>, ICollection<object>>>>(_message.Datas[1]);
                IDictionary<string, Tuple<string, ICollection<object>>> deleteObjs = JsonConvert.DeserializeObject<IDictionary<string, Tuple<string, ICollection<object>>>>(_message.Datas[2]);

                Dictionary<string, List<object>> addIds = new Dictionary<string, List<object>>();

                if (addObjs != null && addObjs.Count > 0)
                {
                    foreach (KeyValuePair<string, Tuple<ICollection<string>, ICollection<object>>> addObj in addObjs)
                    {
                        string tableName = addObj.Key;
                        addIds.Add(tableName, new List<object>());
                        Type type = AbstractProcessor.Types[tableName];
                        if (type == null)
                        {
                            throw new ProcessorException(ResponseCode.SERVER_UNKNOWN_MESSAGE_TYPE, "没有该数据类型！");
                        }
                        ICollection<string> columns = addObj.Value.Item1;
                        IList objs = addObj.Value.Item2.ChangeType(typeof(List<>).MakeGenericType(new Type[]
                        {
                            type
                        })) as IList;

                        var json = await AddMessageProcessor.ProcessData(tableName, objs, columns);

                        addIds[tableName].Add(json);
                    }
                }

                if (updateObjs != null && updateObjs.Count > 0)
                {
                    foreach (KeyValuePair<string, Tuple<ICollection<string>, ICollection<object>>> updateObj in updateObjs)
                    {
                        string tableName = updateObj.Key;
                        Type type = AbstractProcessor.Types[tableName];
                        if (type == null)
                        {
                            throw new ProcessorException(ResponseCode.SERVER_UNKNOWN_MESSAGE_TYPE, "没有该数据类型！");
                        }

                        IList<string> columns = updateObj.Value.Item1.ToList();
                        IList objs = updateObj.Value.Item2.ChangeType(typeof(List<>).MakeGenericType(new Type[]
                        {
                            type
                        })) as IList;

                        var json = await ModifyMessageProcessor.ProcessData(tableName, objs, columns);
                    }
                }

                if (deleteObjs != null && deleteObjs.Count > 0)
                {
                    foreach (KeyValuePair<string, Tuple<string, ICollection<object>>> deleteObj in deleteObjs)
                    {
                        string tableName = deleteObj.Key;
                        Type type = AbstractProcessor.Types[tableName];
                        if (type == null)
                        {
                            throw new ProcessorException(ResponseCode.SERVER_UNKNOWN_MESSAGE_TYPE, "没有该数据类型！");
                        }

                        string id = deleteObj.Value.Item1;
                        var idProperty = type.GetProperties().FirstOrDefault(p => p.Name == id);
                        if (idProperty == null)
                        {
                            throw new ProcessorException(ResponseCode.SERVER_UNKNOWN_MESSAGE_TYPE, "没有该主键类型！");
                        }

                        IList objs = deleteObj.Value.Item2.ChangeType(typeof(List<>).MakeGenericType(new Type[]
                        {
                            idProperty.PropertyType
                        })) as IList;

                        var json = await DeleteMessageProcessor.ProcessData(tableName, objs);                    
                    }
                }


                #endregion

                res.Success = true;
                res.Data = StandardTimeFormatJsonConvertor.SerializeObject(addIds);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.ErrorCode = (int)ResponseCode.SERVER_EXCEPTION;
                res.Msg = ex.Message;
            }

            return res;
        }

    }
}
