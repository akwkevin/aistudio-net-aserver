using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base_Manage
{
    /// <summary>
    /// 应用密钥表
    /// </summary>
    [Table("Base_AppSecret")]
    public class Base_AppSecret : BaseEntity
    {        
        /// <summary>
        /// 应用Id
        /// </summary>
        public String AppId { get; set; }

        /// <summary>
        /// 应用密钥
        /// </summary>
        public String AppSecret { get; set; }

        /// <summary>
        /// 应用名
        /// </summary>
        public String AppName { get; set; }

    }
}