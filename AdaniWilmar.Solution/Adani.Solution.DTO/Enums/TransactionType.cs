using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum TransactionType
    {
        [Description("Credit")] Credit = 1,
        [Description("Debit")] Debit = 2,
    }
}
