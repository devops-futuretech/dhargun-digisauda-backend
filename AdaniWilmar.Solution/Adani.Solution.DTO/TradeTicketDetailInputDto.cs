using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class TradeTicketDetailInputDto
    {
        public TradeTicketDetailInputDto()
        {
            //OilType = new OilTypesDto();
        }
        public long TradeTicketDetailId { get; set; }
        public decimal OilCost { get; set; }
        public decimal Proporion { get; set; }
        public decimal ProcessCost { get; set; }
        //[UIHint("OilTypePartial")]
        //public OilTypesDto OilType { get; set; } 
        public long OilTypeId { get; set; }
        public string OilName { get; set; }
    }

    public class OilTypesDto 
    {
        public long OilTypeId { get; set; }
        public string OilName { get; set; }
        public long VerticleId { get; set; }
    }

}
