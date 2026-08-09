using Adani.Solution.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.Solution.MVC.Models
{
    public class SectionModel : IAPIInputDTO
    {
        public long SectionId { get; set; }
        public string SectionName { get; set; }
        public bool IsActive { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long LoginUserId { get; set; }
    }
}