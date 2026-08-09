using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class HANASaudaNumberListDto
    {
        public List<SaudaNumberDto> SaudaNumberListDto { get; set; }
        public HANASaudaNumberListDto()
        {
            SaudaNumberListDto = new List<SaudaNumberDto>();
        }
    }
    public class SaudaNumberDto
    {
        public long AppId { get; set; }
        public string SaudaNumber { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class HANASaudaApprovalConfirmationDtoList
    {
        public List<SaudaApprovalConfirmationDto> SaudaApprovalConfirmationList { get; set; }
        public HANASaudaApprovalConfirmationDtoList()
        {
            SaudaApprovalConfirmationList = new List<SaudaApprovalConfirmationDto>();
        }
    }

    public class SaudaApprovalConfirmationDto
    {
        public long AppId { get; set; }
        public string SaudaNumber { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }

    
}
