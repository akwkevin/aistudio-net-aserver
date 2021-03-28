using Coldairarrow.Entity.D_Manage;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.D_Manage
{
    public interface ID_NoticeReadingMarksBusiness : IBaseBusiness<D_NoticeReadingMarks>
    {
        Task<PageResult<D_NoticeReadingMarks>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<D_NoticeReadingMarks> GetTheDataAsync(string id);
        Task AddDataAsync(D_NoticeReadingMarks data);
        Task UpdateDataAsync(D_NoticeReadingMarks data);
        Task DeleteDataAsync(List<string> ids);
    }
}