using Adani.Solution.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.Solution.MVC.Models
{
    public class SaudaConversionSKUStatusListModel : SaudaConversionSKUStatusListDto
    {
        public bool PostStatus { get; set; }
        public string PostStatusMessage { get; set; }
    }
}