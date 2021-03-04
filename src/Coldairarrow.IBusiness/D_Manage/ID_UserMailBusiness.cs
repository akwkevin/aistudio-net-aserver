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
        Task<PageResult<D_UserMail>> GetDataListAsync(PageInput<D_UserMailInputDTO> input);
        Task<PageResult<D_UserMail>> GetHistoryDataListAsync(PageInput<D_UserMailInputDTO> input);
        Task<D_UserMail> GetTheDataAsync(string id);
        Task AddDataAsync(D_UserMail data);
        Task UpdateDataAsync(D_UserMail data);
        Task DeleteDataAsync(List<string> ids);
        Task<int> GetReceiveCount(string userId);
    }

    public class D_UserMailInputDTO
    {
        public string condition { get; set; }
        public string keyword { get; set; }
        public string creatorId { get; set; }
        public string userId { get; set; }

        public bool draft { get; set; }

        public DateTime? start { get; set; }
        public DateTime? end { get; set; }

        public bool markflag { get; set; }
    }

    [Map(typeof(D_UserMail))]
    public class D_UserMailDTO : D_UserMail
    {
        public string Avatar { get; set; }
    }
}