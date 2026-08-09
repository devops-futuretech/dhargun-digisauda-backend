using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class AuthorizeOutputDto
    {
        public AuthorizeOutputDto()
        {
            this.UserClaimIds = new List<int>();
            this.FormUsers = new List<FormDto>();
        }
        public List<int> UserClaimIds { get; set; }
        public List<FormDto> FormUsers { get; set; }
        public string LoginToken { get; set; }
        public long UserId { get; set; }
        public string RoleId { get; set; }
        public long RoleTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ProfileName { get; set; }
        public string ProfilePath { get; set; }
        public long? VerticalId { get; set; }
        public long? HeadquartersId { get; set; }
        public long OrganizationReportingToId { get; set; }
        public bool IsApplySpecialityFatDiscount { get; set; }
        //public string Code { get; set; }
        public long StateId { get; set; }
        public string TEG_AuthAPIUrl { get; set; }
        public string TEG_clientId { get; set; }
        public string TEG_clientSecret { get; set; }
    }
}
