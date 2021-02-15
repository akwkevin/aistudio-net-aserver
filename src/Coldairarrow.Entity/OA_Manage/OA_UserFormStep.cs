using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Coldairarrow.Entity.OA_Manage
{
    /// <summary>
    /// OA表单流程
    /// </summary>
    [Table("OA_UserFormStep")]
    public class OA_UserFormStep: BaseEntity
    {
        [MaxLength(50)]
        public string UserFormId { get; set; }
        public string RoleIds { get; set; }
        public string RoleNames { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public string StepName { get; set; }
    }
}
