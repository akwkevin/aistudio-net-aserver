using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_TestController : BaseApiController
    {
        #region DI

        public Base_TestController(IBase_TestBusiness base_TestBus)
        {
            _base_TestBus = base_TestBus;
        }

        IBase_TestBusiness _base_TestBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<Base_Test>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _base_TestBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<Base_Test> GetTheData(IdInputDTO input)
        {
            return await _base_TestBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(Base_Test data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _base_TestBus.AddDataAsync(data);
            }
            else
            {
                await _base_TestBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _base_TestBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}