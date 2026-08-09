using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum FormRemarksType
    {
        [Description("Approval Status Remark")]
        FormApprovalStatusRemark = 1,
        [Description("Form Status Remark")]
        FormStatusRemark = 2
    }
}
