using Coldairarrow.Entity.D_Manage;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.D_Manage
{
    public interface ID_UserMailBusiness : IBaseBusiness<D_UserMail>
    {
        Task<PageResult<D_UserMail>> GetDataListAsync(PageInput<D_UserMailInputDTO> pagination, string condition, string keyword, string userId, string creatorId, bool draft);
        Task<PageResult<D_UserMail>> GetHistoryDataListAsync(PageInput<D_UserMailInputDTO> pagination, string condition, string keyword, string userId, string creatorId, bool draft, bool markflag, DateTime? start = null, DateTime? end = null);
        Task<D_UserMail> GetTheDataAsync(string id);
        Task AddDataAsync(D_UserMail data);
        Task UpdateDataAsync(D_UserMail data);
        Task DeleteDataAsync(List<string> ids);
        Task<int> GetReceiveCount(string userId);
    }

    public class D_UserMailInputDTO
    {
        public string keyword { get; set; }
    }

    [Map(typeof(D_UserMail))]
    public class D_UserMailDTO : D_UserMail
    {
        public string Avatar { get; set; }
    }
}