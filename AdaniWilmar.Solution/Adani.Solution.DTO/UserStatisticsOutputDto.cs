using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserStatisticsOutputDto
    {
        public decimal TotalSaudaQuantity { get; set; }
        public decimal TotalSaudaPercentage { get; set; }
        public decimal AvailableSaudaQuantity { get; set; }
        public decimal OutstandingSaudaQuantity { get; set; }
        public decimal BelowOutstandingSaudaQuantity { get; set; }
        public decimal AboveOutstandingSaudaQuantity { get; set; }
        public decimal PendingSaudaQuantity { get; set; }
        public decimal OverAllSalesQuantity { get; set; }
        public int OverAllSalesPercentage { get; set; }
        public int DealersCount { get; set; }
        public int RankTotalUserCount { get; set; }
        public int LoginUserRank { get; set; }
        public decimal TotalDueForTomorrow { get; set; }
        public decimal TotalOverDue { get; set; }
        public decimal TotalSpecialRateApproval { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public bool IsApplySpecialityFatDiscount { get; set; }
    }
}
