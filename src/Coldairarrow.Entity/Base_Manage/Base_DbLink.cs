using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base_Manage
{
    /// <summary>
    /// 数据库连接表
    /// </summary>
    [Table("Base_DbLink")]
    public class Base_DbLink : BaseEntity
    {
        /// <summary>
        /// 连接名
        /// </summary>
        public String LinkName { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public String ConnectionStr { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public String DbType { get; set; }
    }
}