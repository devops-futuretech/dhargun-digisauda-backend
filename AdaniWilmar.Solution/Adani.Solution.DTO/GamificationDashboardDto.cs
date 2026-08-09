using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class GamificationDashboardDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public long LoginUserId { get; set; }
        public bool IsActive { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public long DistributorId { get; set; }
        public string DistributorCode { get; set; }
        public decimal DistributorTargetMT { get; set; }
        public decimal DistributorAchievementTillN1MT { get; set; }
        public decimal RemainingTargetToAchieveMT { get; set; }
        public long EarnedPoints { get; set; }
        public string CurrentSlab { get; set; }
        public string NextHigherSlab { get; set; }
        public decimal PointsToBeEarnedToReachNextHigherSlab { get; set; }
        public decimal TotalEarningsInRs { get; set; }
        public string SpecialBonusMessage { get; set; }
        public string WholePointsStructure { get; set; }
        public bool IsDiamond { get; set; }
    }

    public class GamificationDashboardImportDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public long LoginUserId { get; set; }
        public string IsActive { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public long DistributorId { get; set; }
        public string DistributorCode { get; set; }
        public decimal DistributorTargetMT { get; set; }
        public decimal DistributorAchievementTillN1MT { get; set; }
        public decimal RemainingTargetToAchieveMT { get; set; }
        public string EarnedPoints { get; set; }
        public string CurrentSlab { get; set; }
        public string NextHigherSlab { get; set; }
        public decimal PointsToBeEarnedToReachNextHigherSlab { get; set; }
        public decimal TotalEarningsInRs { get; set; }
        public string SpecialBonusMessage { get; set; }
        public string WholePointsStructure { get; set; }
        public string IsDiamond { get; set; }
        public string Message { get; set; }
    }

    public class GamificationDashboardDatatableDto
    {
        public string DistributorCode { get; set; }
        public decimal DistributorTargetMT { get; set; }
        public decimal DistributorAchievementTillN1MT { get; set; }
        public decimal RemainingTargetToAchieveMT { get; set; }
        public long EarnedPoints { get; set; }
        public string CurrentSlab { get; set; }
        public string NextHigherSlab { get; set; }
        public decimal PointsToBeEarnedToReachNextHigherSlab { get; set; }
        public decimal TotalEarningsInRs { get; set; }
        public string SpecialBonusMessage { get; set; }
        public bool IsDiamond { get; set; }
    }
}
