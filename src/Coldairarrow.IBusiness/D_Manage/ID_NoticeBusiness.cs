using Coldairarrow.Entity;
using Coldairarrow.Entity.D_Manage;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.D_Manage
{
    public interface ID_NoticeBusiness : IBaseBusiness<D_Notice>
    {
        Task<PageResult<D_NoticeDTO>> GetDataListAsync(PageInput<D_NoticeInputDTO> input);
        Task<D_NoticeDTO> GetTheDataAsync(string id);
        Task AddDataAsync(D_Notice data);
        Task UpdateDataAsync(D_Notice data);
        Task DeleteDataAsync(List<string> ids);

        #region 历史数据查询
        Task<int> GetHistoryDataCountAsync(Input<D_NoticeInputDTO> input);
        Task<List<D_NoticeDTO>> GetHistoryDataListAsync(Input<D_NoticeInputDTO> input);
        Task<PageResult<D_NoticeDTO>> GetPageHistoryDataListAsync(PageInput<D_NoticeInputDTO> input);
        #endregion
    }

    public class D_NoticeInputDTO
    {
        public string condition { get; set; }
        public string keyword { get; set; }
        public string noticeId { get; set; }
        public string userId { get; set; }

        public DateTime? start { get; set; }
        public DateTime? end { get; set; }

        public int status { get; set; }
        public bool markflag { get; set; }
    }

    [Map(typeof(D_Notice))]
    public class D_NoticeDTO : D_Notice
    {
        public string TypeText { get { return ((NoticeType)Type).GetDescription(); } }
        public string StatusText { get { return ((NoticeStatus)Status).GetDescription(); } }
        public string UserId { get; set; }
    }
}