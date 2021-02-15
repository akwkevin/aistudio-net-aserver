using Coldairarrow.Util;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base_Manage
{
    /// <summary>
    /// 系统角色表
    /// </summary>
    [CachType]
    [Table("Base_Role")]
    public class Base_Role : BaseEntity
    {       

        /// <summary>
        /// 角色名
        /// </summary>
        public String RoleName { get; set; }

    }
}