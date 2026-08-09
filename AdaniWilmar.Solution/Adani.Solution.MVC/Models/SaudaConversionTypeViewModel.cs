using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.Solution.DTO;


namespace Adani.Solution.MVC.Models
{
    public class SaudaConversionTypeViewModel
    {
        public List<SaudaConversionTypeDto> ConversionTypes { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public SaudaConversionTypeViewModel()
        {
            ConversionTypes = new List<SaudaConversionTypeDto>();
        }
    }
}