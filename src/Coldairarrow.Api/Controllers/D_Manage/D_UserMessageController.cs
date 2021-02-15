using AIStudio.Service.WebSocketEx;
using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Business.D_Manage;
using Coldairarrow.Entity;
using Coldairarrow.Entity.D_Manage;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.D_Manage
{
    [Route("/D_Manage/[controller]/[action]")]
    public class D_UserMessageController : BaseApiController
    {
        #region DI

        public D_UserMessageController(ID_UserMessageBusiness t_UserMessageBus, IBase_UserBusiness userBus, ID_UserGroupBusiness d_UserGroupBusiness, ICustomWebSocketFactory wsFactory, IOperator ioperator)
        {
            _t_UserMessageBus = t_UserMessageBus;
            _wsFactory = wsFactory;
            _userBus = userBus;
            _d_UserGroupBusiness = d_UserGroupBusiness;
            _operator = ioperator;
        }

        ICustomWebSocketFactory _wsFactory { get; }
        ID_UserMessageBusiness _t_UserMessageBus { get; }
        IBase_UserBusiness _userBus { get; }
        ID_UserGroupBusiness _d_UserGroupBusiness { get; }
        IOperator _operator { get; }
        #endregion

        //#region 获取 
        ///// <summary>
        ///// 获取列表
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<List<D_OnlineUserDTO>> GetDataList()
        //{
        //    var alluser = await _userBus.GetAllOnlineDataList();
        //    var smallAssistant = _wsFactory.GetSmallAssistant();
        //    alluser.Add(new D_OnlineUserDTO(smallAssistant.UserId, smallAssistant.UserName, smallAssistant.Avatar));
        //    var allonline = _wsFactory.AllWithAssistant();

        //    alluser.ForEach(p => { if (allonline.Any(q => q.UserId == p.UserId)){ p.Online = true; } });

        //    var allgroup = (await _d_UserGroupBusiness.GetAllListWhereAsync(p => p.UserIds.Contains(_operator.UserId) || p.CreatorId == _operator.UserId))
        //        .Select(p => 
        //        new D_OnlineUserDTO(
        //            p.Id,
        //        p.Name,
        //        p.Avatar,
        //        true, 
        //        (string.IsNullOrEmpty(p.UserIds)?"^": p.UserIds) + p.CreatorId + "^",
        //        (string.IsNullOrEmpty(p.UserNames) ? "^" : p.UserNames) + p.CreatorName + "^"
        //        ));

        //    alluser.AddRange(allgroup);

        //    return alluser;
        //}

        ///// <summary>
        ///// 获取列表
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<List<D_UserMessageDTO>> GetHistoryDataDialogList(string condition, string keyword, string creatorId, string creatorAvatar, string userId, string userAvatar,bool isGroup, DateTime? start, DateTime? end)
        //{   
        //    var dataList = await _t_UserMessageBus.GetHistoryDataDialogListAsync(condition, keyword, creatorId, creatorAvatar, userId, userAvatar, isGroup, start, end);

        //    for (int i = 0; i < dataList.Count; i++)
        //    {
        //        var p = dataList[i];
        //        p.Avatar = p.CreatorId == _wsFactory.GetSmallAssistant().UserId ? _wsFactory.GetSmallAssistant().Avatar : await _userBus.GetAvatar(p.CreatorId); 
        //        p.Role = _operator?.UserId == p.CreatorId ? "Sender" : "Receiver";
        //        if (i == 0 || (p.CreateTime - dataList[i - 1].CreateTime).TotalSeconds > 60)
        //        {
        //            p.ShowTime = true;
        //        }
        //    }
        //    return dataList;
        //}

        /////// <summary>
        /////// 获取列表,分页
        /////// </summary>
        /////// <returns></returns>
        ////[HttpPost]
        ////public async Task<PageResult<D_UserMessageDTO>> GetHistoryDataList(PageInput<D_UserMessageInputDTO> pagination, string condition, string keyword, string creatorId, string userId, bool markflag, bool? isGroup, DateTime? start, DateTime? end)
        ////{
            
        ////    var dataList = await _t_UserMessageBus.GetHistoryDataListAsync(pagination, condition, keyword, creatorId, userId, markflag, isGroup, start, end);
        ////    var smallAssistant = _wsFactory.GetSmallAssistant();

        ////    userId = _operator?.UserId;
        ////    List<D_UserMessage> updataList = new List<D_UserMessage>();
        ////    for (int i = 0; i < dataList.Count; i++)
        ////    {
        ////        var p = dataList[i];
        ////        p.Avatar = p.CreatorId == _wsFactory.GetSmallAssistant().UserId ? _wsFactory.GetSmallAssistant().Avatar : await _userBus.GetAvatar(p.CreatorId);
        ////        p.Role = _operator?.UserId == p.CreatorId ? "Sender" : "Receiver";
        ////        if (i == 0 || (p.CreateTime - dataList[i - 1].CreateTime).TotalSeconds > 60)
        ////        {
        ////            p.ShowTime = true;
        ////        }
        ////        if (!string.IsNullOrEmpty(creatorId) && !string.IsNullOrEmpty(userId) && p.UserIds.Contains(userId) && !(p.ReadingMarks ?? "").Contains(userId))
        ////        {
        ////            p.ReadingMarks = (string.IsNullOrEmpty(p.ReadingMarks) ? "^" : p.ReadingMarks) + userId + "^";
        ////            p.UpdateEntity();
        ////            updataList.Add(_t_UserMessageBus.MapperBack(p));                   
        ////        }
        ////    }

        ////    if (updataList.Count > 0)
        ////    {
        ////        foreach (var submessages in updataList.GroupBy(p => p.CreateTime.ToString("yyyyMM")))
        ////        {
        ////            List<TableMapperRule> rules = new List<TableMapperRule>()
        ////            {
        ////                new TableMapperRule()
        ////                {
        ////                    MappingType = typeof(D_UserMessage),
        ////                    TableName = typeof(D_UserMessage).Name + submessages.Key,
        ////                }
        ////            };
        ////            await _t_UserMessageBus.GetNewFullService(null, null, rules, true).UpdateAsync(submessages.ToList());
        ////        }
        ////    }

        ////    return dataList;
        ////}
        //#endregion

        #region
        #endregion

        ///// <summary>
        ///// 获取列表,分组
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<List<GroupData>> GetHistoryGroupDataList(string condition, string keyword, string creatorId, string userId, bool markflag, DateTime? start = null, DateTime? end = null)
        //{
        //    var dataList = await _t_UserMessageBus.GetHistoryGroupDataListAsync(condition, keyword, creatorId, userId, markflag, start, end);
        //    dataList.ForEach(async p =>
        //    {
        //        p.Avatar = p.CreatorId == _wsFactory.GetSmallAssistant().UserId ? _wsFactory.GetSmallAssistant().Avatar : await _userBus.GetAvatar(p.CreatorId);
        //    });
        //    return dataList;
        //}
        #region 提交
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<D_UserGroup> GetTheData(string id)
        {
            return await _d_UserGroupBusiness.GetTheDataAsync(id);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">保存的数据</param>
        [HttpPost]
        public async Task SaveData(D_UserGroup data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _d_UserGroupBusiness.AddDataAsync(data);
            }
            else
            {
                UpdateEntity(data);

                await _d_UserGroupBusiness.UpdateDataAsync(data);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids">id数组,JSON数组</param>
        [HttpPost]
        public async Task DeleteData(string ids)
        {
            await _d_UserGroupBusiness.DeleteDataAsync(ids.ToList<string>());
        }

        #endregion
    }
}