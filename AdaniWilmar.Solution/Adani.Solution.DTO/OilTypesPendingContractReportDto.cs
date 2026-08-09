using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class OilTypesPendingContractReportDto
    {
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public List<SkuandPackGroupDto> SkuandPackGroup { get; set; }
        public OilTypesPendingContractReportDto()
        {
            SkuandPackGroup = new List<SkuandPackGroupDto>();
        }
    }
    
    public class SkuandPackGroupDto
    {
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public long PackGroupId { get; set; }
        public string PackGroupName { get; set; }
    }

    public class ListSkuandPackGroupDto
    {
        public List<PackTypeDto> PackGroup { get; set; }
        public List<OilTypesPendingContractReportDto> OilTypesPendingContractReport { get; set; }
        public ListSkuandPackGroupDto()
        {
            PackGroup = new List<PackTypeDto>();
            OilTypesPendingContractReport = new List<OilTypesPendingContractReportDto>();
        }
    }
}
