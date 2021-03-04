using Coldairarrow.Business.OA_Manage;
using Coldairarrow.Entity.OA_Manage;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.OA_Manage
{
    [Route("/OA_Manage/[controller]/[action]")]
    public class OA_UserFormStepController : BaseApiController
    {
        #region DI

        public OA_UserFormStepController(IOA_UserFormStepBusiness oA_UserFormStepBus)
        {
            _oA_UserFormStepBus = oA_UserFormStepBus;
        }

        IOA_UserFormStepBusiness _oA_UserFormStepBus { get; }

        #endregion

        #region 获取

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<OA_UserFormStep>> GetDataList(PageInput<OA_UserFormStepInputDTO> input)
        {
            var dataList = await _oA_UserFormStepBus.GetDataListAsync(input);

            return dataList;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<OA_UserFormStep> GetTheData(IdInputDTO input)
        {
            return await _oA_UserFormStepBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">保存的数据</param>
        [HttpPost]
        public async Task SaveData(OA_UserFormStep data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _oA_UserFormStepBus.AddDataAsync(data);
            }
            else
            {
                UpdateEntity(data);

                await _oA_UserFormStepBus.UpdateDataAsync(data);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids">id数组,JSON数组</param>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _oA_UserFormStepBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}