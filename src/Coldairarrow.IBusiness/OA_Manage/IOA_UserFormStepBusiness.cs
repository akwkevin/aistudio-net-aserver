using Coldairarrow.Entity.OA_Manage;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.OA_Manage
{
    public interface IOA_UserFormStepBusiness : IBaseBusiness<OA_UserFormStep>
    {
        Task<PageResult<OA_UserFormStep>> GetDataListAsync(PageInput<OA_UserFormStepInputDTO> pagination, string condition, string keyword);
        Task<OA_UserFormStep> GetTheDataAsync(string id);
        Task AddDataAsync(OA_UserFormStep data);
        Task UpdateDataAsync(OA_UserFormStep data);
        Task DeleteDataAsync(List<string> ids);       
    }

    public class OA_UserFormStepInputDTO
    {
        public string keyword { get; set; }
    }

    #region 数据模型
    [Map(typeof(OA_UserFormStep))]
    public class OA_UserFormStepDTO : OA_UserFormStep
    {
        public string Avatar { get; set; }
    }
    #endregion
}