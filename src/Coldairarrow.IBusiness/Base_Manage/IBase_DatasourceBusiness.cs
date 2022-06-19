using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Base_Manage
{
    public interface IBase_DatasourceBusiness
    {
        Task<PageResult<Base_Datasource>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<Base_Datasource> GetTheDataAsync(string id);
        Task AddDataAsync(Base_Datasource data);
        Task UpdateDataAsync(Base_Datasource data);
        Task DeleteDataAsync(List<string> ids);
    }
}