using Coldairarrow.Util;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base_Manage
{
    /// <summary>
    /// 部门表
    /// </summary>
    [CachType]
    [Table("Base_Department")]
    public class Base_Department : BaseEntity
    {      
        /// <summary>
        /// 部门名
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 上级部门Id
        /// </summary>
        public String ParentId { get; set; }

        public String ParentIds { get; set; }
        public string ParentNames { get; set; }

        public int Level { get; set; }

    }
}