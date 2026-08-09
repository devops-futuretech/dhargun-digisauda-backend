using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class IssueListDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ComponentId { get; set; }
        public string Component { get; set; }
        public int ImpactId { get; set; }
        public string Impact { get; set; }
        public int FeatureId { get; set; }
        public string Feature { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public long StateId { get; set; }
        public string State { get; set; }
        public string SLA { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public long IssueRaisedBy { get; set; }
        public string IssueRaisedByUserName { get; set; }
        public string ResolvedDateTime { get; set; }
        public string TimeTakenToResolve { get; set; }
        public long DeviceId { get; set; }
        public string IssueFromDevice { get; set; }
        public string IssueComments { get; set; }

        public long LoginUserId { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public List<IssueCommentsDto> Comments { get; set; }
        public List<SupportAttachmentDto> Attachments { get; set; }

        public IssueListDto()
        {
            Comments = new List<IssueCommentsDto>();
            Attachments = new List<SupportAttachmentDto>();
        }
    }
}

