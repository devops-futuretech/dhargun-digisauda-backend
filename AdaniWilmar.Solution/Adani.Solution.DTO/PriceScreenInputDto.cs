using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PriceScreenInputDto
    {
        public long VerticalId { get; set; }
        public List<long> ZonalHeadIds { get; set; }
        public List<long> BDOIds { get; set; }
        public List<long> DealerIds { get; set; }
        public List<long> PlantIds { get; set; }
        public List<long> DeportIds { get; set; }
        public List<long> OilTypeIds { get; set; }
        public long IncotermsId { get; set; }
        public long LoginUserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int PageNo { get; set; }
    }

    public class PriceScreenOutputDto
    {
        public long Id { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }
        public bool IsPublished { get; set; }
    }

    public class PriceScreenDto
    {
        public int ListCount { get; set; }
        public List<PriceScreenOutputDto> PriceScreenList { get; set; }
        
        public PriceScreenDto()
        {
            PriceScreenList = new List<PriceScreenOutputDto>();
        }
    }
}
