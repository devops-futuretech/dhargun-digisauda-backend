using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class BaseGroupMarginDto : IAPIInputDTO
    {
        public BaseGroupMarginDto()
        {
            DerivedGroupMarginList = new List<DerivedGroupMarginDto>();
        }

        public long Id { get; set; }
        public long CustomerGroupId { get; set; }
        public long VerticalId { get; set; }
        public List<long> OilTypeIds { get; set; }
        public long OilTypeId { get; set; }
        public long PackGroupId { get; set; }
        public List<long> StateIds { get; set; }
        public List<long> PackGroupIds { get; set; }
        public decimal Margin { get; set; }
        public bool IsActive { get; set; }
        public long LoginUserId { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public List<DerivedGroupMarginDto> DerivedGroupMarginList { get; set; }
    }

    public class BaseGroupMarginListDto
    {
        public long Id { get; set; }

        public long VerticalId { get; set; }
        public string Vertical { get; set; }

        public long OilTypeId { get; set; }
        public string OilType { get; set; }

        public long CustomerGroupId { get; set; }
        public string CustomerGroup { get; set; }

        public long PackGroupId { get; set; }
        public string PackGroup { get; set; }

        public decimal Margin { get; set; }
        public bool IsActive { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public string StateIds { get; set; }
        public string StateNames { get; set; }
    }

    public class DerivedGroupMarginDto
    {
        public long Id { get; set; }
        public long BaseGroupMarginId { get; set; }
        public long CustomerGroupId { get; set; }
        public string Formula { get; set; }
        public decimal Margin { get; set; }
    }

    public class DerivedGroupMarginListDto
    {
        public long Id { get; set; }
        public long CustomerGroupId { get; set; }
        public string CustomerGroup { get; set; }
        public string Formula { get; set; }
        public decimal Margin { get; set; }
    }

    public class BaseGroupMarginStateListDto
    {
        public long BaseGroupMarginStateId { get; set; }
        public string StateName { get; set; }
        public bool IsActive { get; set; }

    }

    }
