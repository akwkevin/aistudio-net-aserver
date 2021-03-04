using Coldairarrow.Entity.D_Manage;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.D_Manage
{
    public interface ID_UserGroupBusiness : IBaseBusiness<D_UserGroup>
    {
        Task<PageResult<D_UserGroup>> GetDataListAsync(PageInput<D_UserGroupInputDTO> input);
        Task<D_UserGroup> GetTheDataAsync(string id);
        Task AddDataAsync(D_UserGroup data);
        Task UpdateDataAsync(D_UserGroup data);
        Task DeleteDataAsync(List<string> ids);
    }

    public class D_UserGroupInputDTO
    {
        public string condition { get; set; }
        public string keyword { get; set; }
    }
}