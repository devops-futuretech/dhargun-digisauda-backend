using System;
using System.Collections.Generic;
using Adani.Solution.DTO;

namespace Adani.Solution.MVC.Models
{
    public class MonthlyTourPlanViewModel
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long MTPId { get; set; }
        public string EncryptedId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public long DayId { get; set; }
        public string Day { get; set; }
        public long TownId { get; set; }
        public string Town { get; set; }
        public string Area { get; set; }
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public long HeadquartersId { get; set; }
        public string Headquarters { get; set; }
        public string Remarks { get; set; }
        public string HeaderRemarks { get; set; }
        public long LoginUserId { get; set; }
        public long IsEditableForCreatedUser { get; set; }
        public long IsApprover { get; set; }
        public long IsEditableForAdmin { get; set; }
        public string ReasonIds { get; set; }
        public long PJPId { get; set; }
        public long MonthId { get; set; }
        public List<MonthlyTourPlanDetailsDto> MonthlyTourPlanDetailList { get; set; }
        public MonthlyTourPlanViewModel()
        {
            MonthlyTourPlanDetailList = new List<MonthlyTourPlanDetailsDto>();
        }
    }
}