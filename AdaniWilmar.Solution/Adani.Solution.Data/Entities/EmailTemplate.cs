using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class EmailTemplate : Entity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string Template { get; set; }

        [Required]
        public string PlainTemplate { get; set; }
        public string SMSTemplate { get; set; }
        public string SMSTemplateID { get; set; }
    }
}
