using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_DatasourceController : BaseApiController
    {
        #region DI

        public Base_DatasourceController(IBase_DatasourceBusiness base_DatasourceBus)
        {
            _base_DatasourceBus = base_DatasourceBus;
        }

        IBase_DatasourceBusiness _base_DatasourceBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<Base_Datasource>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _base_DatasourceBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<Base_Datasource> GetTheData(IdInputDTO input)
        {
            return await _base_DatasourceBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(Base_Datasource data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _base_DatasourceBus.AddDataAsync(data);
            }
            else
            {
                await _base_DatasourceBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _base_DatasourceBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}