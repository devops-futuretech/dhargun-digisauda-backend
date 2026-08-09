using Adani.Solution.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.Solution.MVC.Models
{
    public class ComplaintApprovalModel : IAPIInputDTO
    {
        public ComplaintApprovalModel()
        {
            ComplaintList = new List<ComplaintApprovalDto>();
        }
        public List<ComplaintApprovalDto> ComplaintList { get; set; }
        public bool PostStatus { get ; set ; }
        public string PostMessage { get ; set ; }
    }
    public class ComplaintFormStatusModel : IAPIInputDTO
    {
        public ComplaintFormStatusModel()
        {
            ComplaintList = new List<ComplaintStatusDto>();
        }
        public List<ComplaintStatusDto> ComplaintList { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}