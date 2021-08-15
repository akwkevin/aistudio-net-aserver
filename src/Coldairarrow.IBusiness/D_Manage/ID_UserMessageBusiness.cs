using Coldairarrow.Entity;
using Coldairarrow.Entity.D_Manage;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.D_Manage
{
    public interface ID_UserMessageBusiness : IShardingBaseBusiness<D_UserMessage>
    {
        Task<PageResult<D_UserMessage>> GetDataListAsync(PageInput<D_UserMessageInputDTO> input);
        Task<D_UserMessage> GetTheDataAsync(string id);
        Task AddDataAsync(D_UserMessage data);
        Task UpdateDataAsync(D_UserMessage data);
        Task DeleteDataAsync(List<string> ids);
        #region 历史数据查询
        Task<int> GetHistoryDataCountAsync(Input<D_UserMessageInputDTO> input);
        Task<List<D_UserMessage>> GetHistoryDataListAsync(Input<D_UserMessageInputDTO> input);
        Task<PageResult<D_UserMessage>> GetPageHistoryDataListAsync(PageInput<D_UserMessageInputDTO> input);
        Task<List<GroupData>> GetHistoryGroupDataListAsync(Input<D_UserMessageInputDTO> input);
        #endregion
    }

    public class D_UserMessageInputDTO
    {

        public string condition { get; set; }
        public string keyword { get; set; }
        public string creatorId { get; set; }
        public string userId { get; set; }

        public bool? isGroup { get; set; }

        public DateTime? start { get; set; }
        public DateTime? end { get; set; }

        public bool markflag { get; set; }
    }


    [Map(typeof(D_UserMessage))]
    public class D_UserMessageDTO : D_UserMessage
    {
        public string Avatar { get; set; }
        public string Role { get; set; }

        public bool ShowTime { get; set; }
    }

    public class D_OnlineUserDTO
    {
        public D_OnlineUserDTO()
        {

        }
        public D_OnlineUserDTO(string userId, string userName, string avatar)
        {
            Avatar = avatar;
            UserName = userName;
            UserId = userId;
        }

        public D_OnlineUserDTO(string groupId, string groupName, string avatar, bool isGroup, string userIds, string userNames) : this(groupId, groupName, avatar)
        {
            IsGroup = isGroup;
            UserIds = userIds;
            UserNames = userNames;
            Online = true;
        }

        public string UserName { get; set; }

        public string UserId { get; set; }
        public string UserNames { get; set; }
        public string UserIds { get; set; }

        public string Avatar { get; set; }

        public bool Online { get; set; }

        public bool IsGroup { get; set; }


        public string LastMessage { get; set; }
        public DateTime? LastDateTime { get; set; }
        //public string LastDateTimeString { get; set; }
        public int Favorite { get; set; }

        public string IP { get; set; }

        public DateTime ConnectedTime { get; set; }

    }
}