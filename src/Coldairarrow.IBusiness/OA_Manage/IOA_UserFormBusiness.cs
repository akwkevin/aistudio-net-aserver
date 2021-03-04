using Coldairarrow.Entity.OA_Manage;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.OA_Manage
{
    public interface IOA_UserFormBusiness : IBaseBusiness<OA_UserForm>
    {
        Task QueueWork(string id);
        Task<string> DequeueWork(string id);
        Task<PageResult<OA_UserFormDTO>> GetDataListAsync(PageInput<OA_UserFormInputDTO> input);
        Task<OA_UserFormDTO> GetTheDataAsync(string id);
        int GetDataListCount(List<string> jsonids, OAStatus status);

        Task AddDataAsync(OA_UserForm data);
        Task UpdateDataAsync(OA_UserForm data);
        Task DeleteDataAsync(List<string> ids);

        #region 历史数据查询
        Task<PageResult<OA_UserForm>> GetPageHistoryDataList(PageInput<OA_UserFormInputDTO> input);
        #endregion

    }

    public class OA_UserFormInputDTO
    {
        public string keyword { get; set; }
        public string condition { get; set; }
        public string userId { get; set; }
        public string applicantUserId { get; set; }
        public string creatorId { get; set; }
        public string alreadyUserIds { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
    }

    [Map(typeof(OA_UserForm))]
    public class OA_UserFormDTO : OA_UserForm
    {
        public string ApplicantUserAndDepartment { get => ApplicantUser + "-" + ApplicantDepartment; }
        public string UserNamesAndRoles
        {
            get
            {
                var users = (UserNames ?? "").Replace("^", " ").Trim().Replace(" ", ",");
                var roles = (UserRoleNames ?? "").Replace("^", " ").Trim().Replace(" ", ",");
                return users + (string.IsNullOrEmpty(roles) ? "" : "-" + roles);
            }
        }

        public string Current
        {
            get { return CurrentNode?.Replace("^", "").Trim().Replace(" ", ","); }
        }


        public string ExpectedDateString { get => ExpectedDate?.ToString("yyyy-MM-dd"); }

        public string WorkflowJSON { get; set; }

        public List<OA_UserFormStepDTO> Comments { get; set; }

        public List<OAStep> Steps { get; set; }

        public int CurrentStepIndex { get; set; }
        public string CurrentStepId { get; set; }
        public string Avatar { get; set; }
    }
}