using Coldairarrow.Util;
using EFCore.Sharding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Coldairarrow.Entity.D_Manage
{
    /// <summary>
    /// 系统消息表
    /// </summary>
    [PhysicDeleteType]
    [PushMessageType]
    [BatchSaveType]
    [ExpandByDateModeType(ExpandByDateMode.PerMonth)]
    [Table("D_UserMessage")]
    public class D_UserMessage : MessageBaseEntity
    {
        public int Type { get; set; }
        public string ReadingMarks { get; set; }
        public string GroupId { get; set; }

        public string GroupName { get; set; }
    }
}
