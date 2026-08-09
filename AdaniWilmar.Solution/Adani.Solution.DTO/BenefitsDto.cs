using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class BenefitsDto : LoginUserIdDto,IAPIInputDTO
    {
        public long Id { get; set; }
        public string BenefitCategory { get; set; }
        public long BenefitDays { get; set; }
        public long BenefitTypeId { get; set; }
        public string BenefitType { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }

        public List<long> SelectedBenefitIdsToRemove { get; set; }
        public string SelectedBenefitIdsToRemoveString { get; set; }

        public List<BenefitsDto> BenefitsDtoList { get; set; }
    }
}
