using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class GamificationDashboard : Auditable
    {
        public long DistributorId { get; set; }
        public string DistributorCode { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal DistributorTargetMT { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal DistributorAchievementTillN1MT { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal RemainingTargetToAchieveMT { get; set; }
        public long EarnedPoints { get; set; }  
        public string CurrentSlab { get; set; }
        public string NextHigherSlab { get; set; }
        public decimal PointsToBeEarnedToReachNextHigherSlab { get; set; }
        public decimal TotalEarningsInRs { get; set; }
        public string SpecialBonusMessage { get; set; }
        public string WholePointsStructure { get; set; }
        public bool IsActive { get; set; }
        public bool IsDiamond { get; set; }
    }
}
