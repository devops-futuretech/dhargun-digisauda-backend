namespace Adani.Solution.DTO
{
    public class SkuSalesFilterDto:UserIdDto
    {
        public long SkuId { get; set; }
        public int UomId { get; set; }
    }
}
