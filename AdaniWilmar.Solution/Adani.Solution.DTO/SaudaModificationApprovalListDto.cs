using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SaudaModificationApprovalListDto
    {
        public long ListCount { get; set; }
        public List<SaudaModificationListItemDto> Items { get; set; }

        public SaudaModificationApprovalListDto()
        {
            Items = new List<SaudaModificationListItemDto>();
        }
    }
}


