using AIStudio.Service.WebSocketEx;
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

        public D_UserMailController(ID_UserMailBusiness t_UserMailBus, ICustomWebSocketMessageHandler customWebSocketMessageHandler, IOperator __operator, IBase_UserBusiness userBus)
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
        #endregion

        #region 获取

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="condition">查询字段</param>
        /// <param name="keyword">关键字</param>
        /// <param name="userId"></param>
        /// <param name="creatorId"></param>
        /// <param name="draft"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<D_UserMail>> GetDataList(PageInput<D_UserMailInputDTO> pagination, string condition, string keyword, string userId, string creatorId, bool draft)
        {
            var dataList = await _t_UserMailBus.GetDataListAsync(pagination, condition, keyword, userId, creatorId, draft);

            return dataList;
        }

        ///// <summary>
        ///// 获取列表，历史分页
        ///// </summary>
        ///// <param name="pagination"></param>
        ///// <param name="condition"></param>
        ///// <param name="keyword"></param>
        ///// <param name="userId"></param>
        ///// <param name="creatorId"></param>
        ///// <param name="draft"></param>
        ///// <param name="markflag"></param>
        ///// <param name="start"></param>
        ///// <param name="end"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<AjaxResult<List<D_UserMailDTO>>> GetHistoryDataList(PageInput<D_UserMailInputDTO> pagination, string condition, string keyword, string userId, string creatorId, bool draft, bool markflag, DateTime? start, DateTime? end)
        //{
        //    var dataList = Mapper.Map<List<D_UserMailDTO>>(await _t_UserMailBus.GetHistoryDataListAsync(pagination, condition, keyword, userId, creatorId, draft, markflag, start, end));
        //    dataList.ForEach(async p =>
        //    {
        //        p.Avatar = await _userBus.GetAvatar(p.CreatorId);
        //    });

        //    return DataTable(dataList, pagination);
        //}

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
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<D_UserMail> GetTheData(string id)
        {
            var mail = await _t_UserMailBus.GetTheDataAsync(id);
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