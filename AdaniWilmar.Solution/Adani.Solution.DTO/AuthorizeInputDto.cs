namespace Adani.Solution.DTO
{
    public class AuthorizeInputDto
    {
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsRequestFromWeb { get; set; }
        public long VerticalId { get; set; }
    }
}
