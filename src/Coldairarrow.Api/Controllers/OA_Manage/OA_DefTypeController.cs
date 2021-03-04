using Coldairarrow.Business.OA_Manage;
using Coldairarrow.Entity.OA_Manage;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.OA_Manage
{
    [Route("/OA_Manage/[controller]/[action]")]
    public class OA_DefTypeController : BaseApiController
    {
        #region DI

        public OA_DefTypeController(IOA_DefTypeBusiness oA_DefTypeBus)
        {
            _oA_DefTypeBus = oA_DefTypeBus;
        }

        IOA_DefTypeBusiness _oA_DefTypeBus { get; }

        #endregion

        #region 获取

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult<List<OA_DefType>>> GetDataList(PageInput<OA_DefTypeInputDTO> input)
        {
            var dataList = await _oA_DefTypeBus.GetDataListAsync(input);

            return dataList;
        }

        [HttpPost]
        public async Task<List<OA_DefTypeDTO>> GetTreeDataList(string keyword)
        {
            return await _oA_DefTypeBus.GetTreeDataListAsync(keyword);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<OA_DefType> GetTheData(IdInputDTO input)
        {
            return await _oA_DefTypeBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">保存的数据</param>
        [HttpPost]
        public async Task SaveData(OA_DefType data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _oA_DefTypeBus.AddDataAsync(data);
            }
            else
            {
                UpdateEntity(data);

                await _oA_DefTypeBus.UpdateDataAsync(data);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids">id数组,JSON数组</param>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _oA_DefTypeBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}