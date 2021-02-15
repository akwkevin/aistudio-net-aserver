using Coldairarrow.Entity.OA_Manage;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.OA_Manage
{
    public interface IOA_DefFormBusiness : IBaseBusiness<OA_DefForm>
    {
        Task<List<OA_DefFormTree>> GetTreeDataListAsync(string type, List<string> roleidlist);
        Task<PageResult<OA_DefFormDTO>> GetDataListAsync(PageInput<OA_DefFormInputDTO> pagination, string condition, string keyword);
        Task<OA_DefFormDTO> GetTheDataAsync(string id);
        Task AddDataAsync(OA_DefForm data);
        Task UpdateDataAsync(OA_DefForm data);
        Task DeleteDataAsync(List<string> ids);
    }

    public class OA_DefFormInputDTO
    {
        public string keyword { get; set; }
    }

    [Map(typeof(OA_DefForm))]
    public class OA_DefFormDTO : OA_DefForm
    {
        public string[] ValueRoles
        {
            get { return Value?.Split(new string[] { "^" }, System.StringSplitOptions.RemoveEmptyEntries); }
            set
            {
                if (value != null)
                {
                    Value = "^" + string.Join("^", value) + "^";
                }
                else
                {
                    Value = null;
                }
            }
        }
    }

    public class OA_DefFormTree : TreeModel
    {
        public object children { get => Children; }
        public string title { get => Text; }
        public string text { get => Text; }
        public string value { get => Id; }
        public string key { get => Id; }

        public object scopedSlots { get; set; }

        public int jsonVersion { get; set; }
        public string jsonId { get; set; }
        public string json { get; set; }

        public string type { get; set; }
    }
}