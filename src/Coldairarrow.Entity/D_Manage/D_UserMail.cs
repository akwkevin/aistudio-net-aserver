using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Coldairarrow.Entity.D_Manage
{
    /// <summary>
    /// 系统邮件表
    /// </summary>
    [PushMessageType]
    [Table("D_UserMail")]
    public class D_UserMail : MessageBaseEntity
    {
        public string Title { get; set; }   
        public UserMailType Type { get; set; }
        public string CCIds { get; set; }
        public string CCNames { get; set; }
        public string ReadingMarks { get; set; }
        public bool StarMark { get; set; }
        public string Appendix { get; set; }

        /// <summary>
        /// 状态 =0草稿中，=1已发布，=2撤回
        /// </summary>
        public EmailStatus Status { get; set; }
    }
}
