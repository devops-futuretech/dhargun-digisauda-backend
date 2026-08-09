namespace Adani.Solution.DTO
{
    public class LiftingRequestListInputDto:LoginUserIdDto
    {
        public int StatusId { get; set; }
        public long BDOId { get; set; }
        public long ZHId { get; set; }
    }
}
