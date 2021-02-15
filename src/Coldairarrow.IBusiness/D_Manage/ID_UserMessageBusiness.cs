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
        Task<PageResult<D_UserMessage>> GetDataListAsync(PageInput<D_UserMessageInputDTO> pagination, string condition, string keyword);
        Task<D_UserMessage> GetTheDataAsync(string id);
        Task AddDataAsync(D_UserMessage data);
        Task UpdateDataAsync(D_UserMessage data);
        Task DeleteDataAsync(List<string> ids);
        //Task<List<D_UserMessageDTO>> GetHistoryDataDialogListAsync(string condition, string keyword, string creatorId, string creatorAvatar, string userId, string userAvatar, bool isGroup, DateTime? start = null, DateTime? end = null);
        //Task<PageResult<D_UserMessageDTO>> GetHistoryDataListAsync(PageInput<D_UserMessageInputDTO> pagination, string condition, string keyword, string creatorId, string userId, bool markflag, bool? isGroup, DateTime? start = null, DateTime? end = null);
        //Task<List<GroupData>> GetHistoryGroupDataListAsync(string condition, string keyword, string creatorId, string userId, bool markflag, DateTime? start = null, DateTime? end = null);
    }

    public class D_UserMessageInputDTO
    {
        public string keyword { get; set; }
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

    }
}