using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Console.Common
{
    public class NumberFormat
    {
        public static decimal DecimalFormat2(decimal? value)
        {
            return Convert.ToDecimal(string.Format("{0:0.00}", (value ?? 0)));
        }
    }
}
