using System;
using System.Collections.Generic;
namespace Adani.Solution.DTO
{
    public class LiftingDetailViewDto
    {
        public decimal CompletedQuantity { get; set; }
        public decimal InprogressQuantity { get; set; }
        public decimal PendingQuantity { get; set; }
        public decimal PendingQuantityCase { get; set; }
        public IList<SaudaOrderDetails> LiftedSkus { get; set; }
        public LiftingDetailViewDto()
        {
            LiftedSkus = new List<SaudaOrderDetails>();
        }
    }
}
