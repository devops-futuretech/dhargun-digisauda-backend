using Adani.Solution.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.Solution.MVC.Models
{
    public class SaudaApprovalViewModel : IAPIInputDTO
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long SaudaId { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public long LoginUserId { get; set; }
    }
}