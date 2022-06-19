using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base_Manage
{
    /// <summary>
    /// 数据源表
    /// </summary>
    [Table("Base_Datasource")]
    public class Base_Datasource : BaseEntity
    { 
        /// <summary>
        /// 编号
        /// </summary>
        [Description("编号")]
        public String Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        public String Name { get; set; }

        /// <summary>
        /// 数据库主键
        /// </summary>
        [Description("数据库主键")]
        public String DbLinkId { get; set; }

        /// <summary>
        /// sql语句
        /// </summary>
        [Description("sql语句")]
        public String Sql { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public String Description { get; set; } 
    }
}