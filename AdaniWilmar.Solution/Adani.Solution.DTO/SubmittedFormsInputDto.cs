using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SubmittedFormsInputDto : LoginUserIdDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsDependentForm { get; set; }
        public string SearchText { get; set; }
        public int StartIndex { get; set; }
        public int PageSize { get; set; }

        public long CustomerId { get; set; }
        public long EmployeeId { get; set; }
        public long SkuId { get; set; }
        public long PlantId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int StatusId { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public long DealerId { get; set; }
    }
}
