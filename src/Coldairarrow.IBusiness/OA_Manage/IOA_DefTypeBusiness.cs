using Coldairarrow.Entity.OA_Manage;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.OA_Manage
{
    public interface IOA_DefTypeBusiness : IBaseBusiness<OA_DefType>
    {
        Task<PageResult<OA_DefType>> GetDataListAsync(PageInput<OA_DefTypeInputDTO> pagination, string condition, string keyword);
        Task<OA_DefType> GetTheDataAsync(string id);
        Task AddDataAsync(OA_DefType data);
        Task UpdateDataAsync(OA_DefType data);
        Task DeleteDataAsync(List<string> ids);
        Task<List<OA_DefTypeDTO>> GetTreeDataListAsync(string type = null);
    }

    public class OA_DefTypeInputDTO
    {
        public string keyword { get; set; }
    }

    public class OA_DefTypeDTO : TreeModel
    {
        public string RealId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Unit { get; set; }

        public object children { get => Children; }
        public string title { get => Text; }
        public string value { get => Id; }
        public string key { get => Id; }
    }
}