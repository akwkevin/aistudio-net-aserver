using AIStudio.Service.WebSocketEx;
using AutoMapper;
using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Business.D_Manage;
using Coldairarrow.Entity.D_Manage;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.D_Manage
{
    [Route("/D_Manage/[controller]/[action]")]
    public class D_UserMailController : BaseApiController
    {
        #region DI

        public D_UserMailController(ID_UserMailBusiness t_UserMailBus, ICustomWebSocketMessageHandler customWebSocketMessageHandler, IOperator __operator, IBase_UserBusiness userBus, IMapper mapper)
        {
            _t_UserMailBus = t_UserMailBus;
            _userBus = userBus;
            _customWebSocketMessageHandler = customWebSocketMessageHandler;
            _operator = __operator;
        }

        ID_UserMailBusiness _t_UserMailBus { get; }
        ICustomWebSocketMessageHandler _customWebSocketMessageHandler { get; }
        IBase_UserBusiness _userBus { get; }
        IOperator _operator { get; }

        IMapper _mapper { get; }

        #endregion

        #region 获取

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<D_UserMail>> GetDataList(PageInput<D_UserMailInputDTO> input)
        {
            var dataList = await _t_UserMailBus.GetDataListAsync(input);

            return dataList;
        }

        /// <summary>
        /// 获取列表，历史分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<D_UserMailDTO>> GetHistoryDataList(PageInput<D_UserMailInputDTO> input)
        {
            var dataList = await _t_UserMailBus.GetHistoryDataListAsync(input);

            var data = new PageResult<D_UserMailDTO>() { Total = dataList.Total, Data = _mapper.Map<List<D_UserMailDTO>>(dataList.Data) };
            data.Data.ForEach(async p =>
            {
                p.Avatar = await _userBus.GetAvatar(p.CreatorId);
            });

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> GetReceiveCount(string userId)
        {
            return await _t_UserMailBus.GetReceiveCount(userId);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<D_UserMail> GetTheData(IdInputDTO input)
        {
            var mail = await _t_UserMailBus.GetTheDataAsync(input.id);
            string userId = _operator?.UserId;
            if (!string.IsNullOrEmpty(userId) && mail.UserIds.Contains(userId) && !(mail.ReadingMarks ?? "").Contains(userId))
            {
                mail.ReadingMarks = (string.IsNullOrEmpty(mail.ReadingMarks) ? "^" : mail.ReadingMarks) + userId + "^";
                UpdateEntity(mail);
                await _t_UserMailBus.UpdateDataAsync(mail);
            }
            return mail;
        }

        #endregion

        #region 提交

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">保存的数据</param>
        [HttpPost]
        public async Task SaveData(D_UserMail data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _t_UserMailBus.AddDataAsync(data);
            }
            else
            {
                UpdateEntity(data);

                await _t_UserMailBus.UpdateDataAsync(data);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids">id数组,JSON数组</param>
        [HttpPost]
        public async Task DeleteData(string ids)
        {
            await _t_UserMailBus.DeleteDataAsync(ids.ToList<string>());
        }

        #endregion
    }
}