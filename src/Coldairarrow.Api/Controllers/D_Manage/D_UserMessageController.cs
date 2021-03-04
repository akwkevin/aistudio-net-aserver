using AIStudio.Service.WebSocketEx;
using AutoMapper;
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

        public D_UserMessageController(ID_UserMessageBusiness t_UserMessageBus, IBase_UserBusiness userBus, ID_UserGroupBusiness d_UserGroupBusiness, ICustomWebSocketFactory wsFactory, IOperator ioperator, IMapper mapper)
        {
            _t_UserMessageBus = t_UserMessageBus;
            _wsFactory = wsFactory;
            _userBus = userBus;
            _d_UserGroupBusiness = d_UserGroupBusiness;
            _operator = ioperator;
            _mapper = mapper;
        }

        ICustomWebSocketFactory _wsFactory { get; }
        ID_UserMessageBusiness _t_UserMessageBus { get; }
        IBase_UserBusiness _userBus { get; }
        ID_UserGroupBusiness _d_UserGroupBusiness { get; }
        IOperator _operator { get; }
        IMapper _mapper { get; }
        #endregion

        #region 获取 
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<D_OnlineUserDTO>> GetUserList()
        {
            var alluser = (await _userBus.GetListAsync()).Select(p => new D_OnlineUserDTO(p.Id, p.UserName, p.Avatar)).ToList(); ;
            var smallAssistant = _wsFactory.GetSmallAssistant();
            alluser.Add(new D_OnlineUserDTO(smallAssistant.UserId, smallAssistant.UserName, smallAssistant.Avatar));
            var allonline = _wsFactory.AllWithAssistant();

            alluser.ForEach(p => { if (allonline.Any(q => q.UserId == p.UserId)) { p.Online = true; } });

            var allgroup = (await _d_UserGroupBusiness.GetIQueryable().Where(p => p.UserIds.Contains(_operator.UserId) || p.CreatorId == _operator.UserId).ToListAsync())
                .Select(p =>
                new D_OnlineUserDTO(
                    p.Id,
                p.Name,
                p.Avatar,
                true,
                (string.IsNullOrEmpty(p.UserIds) ? "^" : p.UserIds) + p.CreatorId + "^",
                (string.IsNullOrEmpty(p.UserNames) ? "^" : p.UserNames) + p.CreatorName + "^"
                ));

            alluser.AddRange(allgroup);

            return alluser;
        }

        /// <summary>
        /// 获取列表，按时间获取
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<D_UserMessageDTO>> GetHistoryDataList(PageInput<D_UserMessageInputDTO> input)
        {
            var dataList = _mapper.Map<List<D_UserMessageDTO>>(await _t_UserMessageBus.GetHistoryDataListAsync(input));

            for (int i = 0; i < dataList.Count; i++)
            {
                var p = dataList[i];
                p.Avatar = p.CreatorId == _wsFactory.GetSmallAssistant().UserId ? _wsFactory.GetSmallAssistant().Avatar : await _userBus.GetAvatar(p.CreatorId);
                p.Role = _operator?.UserId == p.CreatorId ? "Sender" : "Receiver";
                if (i == 0 || (p.CreateTime - dataList[i - 1].CreateTime).TotalSeconds > 60)
                {
                    p.ShowTime = true;
                }
            }
            return dataList;
        }

        /// <summary>
        /// 获取列表,分页，按未读标记,上下滑动的时候使用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<D_UserMessageDTO>> GetPageHistoryDataList(PageInput<D_UserMessageInputDTO> input)
        {
            var result = await _t_UserMessageBus.GetPageHistoryDataListAsync(input);

            var dataList = _mapper.Map<List<D_UserMessageDTO>>(result.Data);
            var smallAssistant = _wsFactory.GetSmallAssistant();

            var userId = _operator?.UserId;
            List<D_UserMessage> updataList = new List<D_UserMessage>();
            for (int i = 0; i < dataList.Count; i++)
            {
                var p = dataList[i];
                p.Avatar = p.CreatorId == _wsFactory.GetSmallAssistant().UserId ? _wsFactory.GetSmallAssistant().Avatar : await _userBus.GetAvatar(p.CreatorId);
                p.Role = _operator?.UserId == p.CreatorId ? "Sender" : "Receiver";
                if (i == 0 || (p.CreateTime - dataList[i - 1].CreateTime).TotalSeconds > 60)
                {
                    p.ShowTime = true;
                }
                if (!string.IsNullOrEmpty(userId) && p.UserIds.Contains(userId) && !(p.ReadingMarks ?? "").Contains(userId))
                {
                    p.ReadingMarks = (string.IsNullOrEmpty(p.ReadingMarks) ? "^" : p.ReadingMarks) + userId + "^";
                    UpdateEntity(p);
                    updataList.Add(p);
                }
            }

            if (updataList.Count > 0)
            {
                await _t_UserMessageBus.UpdateAsync(updataList);
            }

            return new PageResult<D_UserMessageDTO>() { Total = result.Total, Data = dataList };
        }
        #endregion

        #region
        #endregion

        /// <summary>
        /// 获取列表,分组，未读标记
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<GroupData>> GetHistoryGroupDataList(PageInput<D_UserMessageInputDTO> input)
        {
            var dataList = await _t_UserMessageBus.GetHistoryGroupDataListAsync(input);
            dataList.ForEach(async p =>
            {
                p.Avatar = p.CreatorId == _wsFactory.GetSmallAssistant().UserId ? _wsFactory.GetSmallAssistant().Avatar : await _userBus.GetAvatar(p.CreatorId);
            });
            return dataList;
        }
        #region 提交
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<D_UserGroup> GetTheData(IdInputDTO input)
        {
            return await _d_UserGroupBusiness.GetTheDataAsync(input.id);
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