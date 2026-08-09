using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ZonalHeadDto : LoginUserIdDto
    {
        public List<long> ZHIds { get; set; }
    }

    public class NationalHeadDto : LoginUserIdDto
    {
        public List<long> NHIds { get; set; }
    }

    public class ZonalHeadMappingDto : IAPIInputDTO
    {
        public List<int> ZoneIds { get; set; }
        public List<int> StateIds { get; set; }
       
        public int ZonalHeadId { get; set; }
        public string ZonalHeadName { get; set; }
        public bool IsActive { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public List<long> ZHIds { get; set; }

        public int ZoneId { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }

       
    }

    public class OilTypeMappingDto : IAPIInputDTO
    {
        public List<long> ZoneIds { get; set; }
        public List<long> StateIds { get; set; }
        public List<long> ZHIds { get; set; }
        public List<long> BDOIds { get; set; }
        public long VerticalId { get; set; }
        public List<long> OilTypeIds { get; set; }

        public int OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public bool IsActive { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }
    }
