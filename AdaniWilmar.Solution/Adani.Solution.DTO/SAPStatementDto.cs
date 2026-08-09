using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SAPStatementDto
    {
        public List<statement> statement { get; set; }
    }
    public class statement
    {
        public long AccountStatementId { get; set; }
        public string compCode { get; set; }
        public string customer { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string spGL_A { get; set; }
        public string spGL_H { get; set; }
        public string format { get; set; }
    }
}
