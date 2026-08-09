using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SubmittedForm : Auditable
    {
        public SubmittedForm()
        {
            this.SubmittedFormQuestions = new HashSet<SubmittedFormQuestion>();
        }
        /// <summary>
        /// Complaint details against - Customer Id
        /// </summary>   
        [ForeignKey("Retailer")]
        public long? UserId { get; set; }
        public string CustomerName { get; set; }
        [Required]
        public long FormId { get; set; }
        public string FormName { get; set; }
        public bool IsFormStatus { get; set; }
        [ForeignKey("FormStatus")]
        public int? FormStatusId { get; set; }
        public long? FormApprovalStatusId { get; set; }
        public long? ParentFormId { get; set; } //If type is feedback
        public long? DemoUserId { get; set; }
        public long? DemoId { get; set; }
        [MaxLength(4000)]
        public string Remarks { get; set; }
        /// <summary>
        /// Dealer Id 
        /// </summary>
        public long? DealerId { get; set; }
        public string DealerName { get; set; }
        public virtual Form Form { get; set; }
        public virtual FormStatus FormStatus { get; set; }
        public virtual Retailer Retailer { get; set; }
        public virtual ICollection<SubmittedFormQuestion> SubmittedFormQuestions { get; set; }
    }
}
