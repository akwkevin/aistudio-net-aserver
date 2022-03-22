using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Base_Manage
{
    public interface IBase_TestBusiness
    {
        Task<PageResult<Base_Test>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<Base_Test> GetTheDataAsync(string id);
        Task AddDataAsync(Base_Test data);
        Task UpdateDataAsync(Base_Test data);
        Task DeleteDataAsync(List<string> ids);
    }
}