namespace Adani.Solution.DTO
{
    public class FormDto
    {
        public long Id { get; set; }
        public long FormId { get; set; }
        public string FormName { get; set; }        
        public bool IsActive { get; set; }
        public bool IsFormStatus { get; set; }
        public string RoleIds { get; set; }
        public long ParentFormId { get; set; }
        public string ParentFormName { get; set; }
        public bool IsSubmittedForms { get; set; }
    }
}
