using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum PageType
    {
        [Description("Competitor")] Competitor = 1,
        [Description("ProspectiveDealer")] ProspectiveDealer = 2,
        [Description("Dealer")] Dealer = 3,
        [Description("Support")] Support = 4,
        [Description("DynamicFormAttachments")] DynamicFormAttachments = 5,
        [Description("AudioFiles")]  AudioFiles = 6,
        [Description("ConsentImages")] ConsentImages = 7,
        [Description("ImagesSaudaMappingwithCallRecording")] ImagesSaudaMappingwithCallRecording = 8,
        [Description("FinalPriceDownload")] FinalPriceDownload = 9,
        [Description("ProfilePhotos")] ProfilePhotos = 10,

    }
}
