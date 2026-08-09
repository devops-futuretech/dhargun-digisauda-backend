using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ComplaintApprovalDto
    {
        public long ComplaintId { get; set; }
        public string CustomerName { get; set; }
        public string DealerName { get; set; }
        public string FormName { get; set; }        
        public string SalesExecutiveName { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ApprovalStatusId { get; set; }
        public string ApprovalStatus { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }

    public class ComplaintStatusDto
    {
        public long ComplaintId { get; set; }
        public string CustomerName { get; set; }
        public string DealerName { get; set; }
        public string FormName { get; set; }
        public string SubmitedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }

    public class ComplaintApprovalInputDto : LoginUserIdDto
    {
        public long ComplaintId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
    public class ComplaintApprovalListInputDto : LoginUserIdDto
    {
        public ComplaintApprovalListInputDto()
        {
            approvallist = new List<ComplaintApprovalInputDto>(); 
        }
        public IList<ComplaintApprovalInputDto> approvallist { get; set; }
    }
}
