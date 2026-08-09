namespace Adani.Solution.DTO
{
    public class CreditLimitTotalDto
    {
        public int DealersCount { get; set; }
        public decimal TotalCreditLimit { get; set; }
        public decimal TotalCreditExposure { get; set; }
        public decimal TotalPack { get; set; }
        //public decimal TotalBulkPack { get; set; }
        //public decimal TotalCustomPack { get; set; }
    }
}
