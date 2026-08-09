namespace Adani.Solution.DTO
{
    public class RoleDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long RoleTypeId { get; set; }
        public long SuperRoleTypeId { get; set; }
    }
}
