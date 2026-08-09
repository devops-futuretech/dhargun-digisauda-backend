using Adani.Solution.DTO;
using System.Collections.Generic;

namespace Adani.Solution.MVC.Models
{
    public class ConfigurationViewModel
    {
        public List<ConfigurationDto> Configurations { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public ConfigurationViewModel()
        {
            Configurations = new List<ConfigurationDto>();
        }
    }


    public class BiddingWindowDashboardViewModel
    {
        public List<BiddingWindowDashboardDto> BiddingWindowDahboard { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public BiddingWindowDashboardViewModel()
        {
            BiddingWindowDahboard = new List<BiddingWindowDashboardDto> ();
        }
    }
}