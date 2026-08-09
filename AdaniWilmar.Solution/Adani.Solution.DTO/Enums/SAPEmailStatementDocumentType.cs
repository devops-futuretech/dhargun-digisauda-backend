using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum SAPEmailStatementDocumentType
    {
        [Description("PDF")] PDF = 1,
    
        [Description("EXCEL")] EXCEL = 2
    }
}
