using Coldairarrow.Business.D_Manage;
using Coldairarrow.Entity;
using Coldairarrow.Entity.D_Manage;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.D_Manage
{
    [Route("/D_Manage/[controller]/[action]")]
    public class D_NoticeController : BaseApiController
    {
        #region DI

        public D_NoticeController(ID_NoticeBusiness d_NoticeBus, ID_NoticeReadingMarksBusiness d_NoticeReadingMarksBusiness)
        {
            _d_NoticeBus = d_NoticeBus;
            _d_NoticeReadingMarksBusiness = d_NoticeReadingMarksBusiness;
        }

        ID_NoticeBusiness _d_NoticeBus { get; }
        ID_NoticeReadingMarksBusiness _d_NoticeReadingMarksBusiness { get; }
        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<D_NoticeDTO>> GetDataList(PageInput<D_NoticeInputDTO> input)
        {
            return await _d_NoticeBus.GetDataListAsync(input);
        }

        /// <summary>
        /// 获取列表，历史分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<D_NoticeDTO>> GetPageHistoryDataList(PageInput<D_NoticeInputDTO> input)
        {
            var dataList = await _d_NoticeBus.GetPageHistoryDataListAsync(input);

            return dataList;
        }

        [HttpPost]
        public async Task<D_NoticeDTO> GetTheData(IdInputDTO input)
        {            
            var data = await _d_NoticeBus.GetTheDataAsync(input.id);

            //插入已读标记
            if (string.IsNullOrEmpty(data.UserId) && data.Status == NoticeStatus.Published)
                await AddReadingMark(data);

            return data;
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(D_Notice data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _d_NoticeBus.AddDataAsync(data);
            }
            else
            {
                await _d_NoticeBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _d_NoticeBus.DeleteDataAsync(ids);
        }

        public async Task AddReadingMark(D_Notice data)
        {
            D_NoticeReadingMarks readingMarks = new D_NoticeReadingMarks() { NoticeId = data.Id };
            InitEntity(readingMarks);
            await _d_NoticeReadingMarksBusiness.AddDataAsync(readingMarks);
        }
        #endregion
    }
}