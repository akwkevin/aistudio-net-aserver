using Coldairarrow.Util;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base_Manage
{
    /// <summary>
    /// 系统用户表
    /// </summary>
    [CachType]
    [Table("Base_User")]
    public class Base_User : BaseEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public String RealName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 所属部门Id
        /// </summary>
        public String DepartmentId { get; set; }

        [MaxLength(255)]
        public string PhoneNumber { get; set; }

        [MaxLength(500)]
        public string Avatar { get; set; }
    }

    public enum Sex
    {
        [Description("男人")]
        Man = 1,

        [Description("女人")]
        Woman = 0
    }
}