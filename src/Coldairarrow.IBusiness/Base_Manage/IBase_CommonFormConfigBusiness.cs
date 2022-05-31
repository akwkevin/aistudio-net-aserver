using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Base_Manage
{
    public interface IBase_CommonFormConfigBusiness : IBaseBusiness<Base_CommonFormConfig>
    {
        Task<PageResult<Base_CommonFormConfig>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<Base_CommonFormConfig> GetTheDataAsync(string id);
        Task AddDataAsync(Base_CommonFormConfig data);
        Task UpdateDataAsync(Base_CommonFormConfig data);
        Task DeleteDataAsync(List<string> ids);
    }
}