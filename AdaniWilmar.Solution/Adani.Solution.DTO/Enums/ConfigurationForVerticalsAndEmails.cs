using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum ConfigurationForVerticalsAndEmails
    {
        [Description("Divisions Based On Sauda Validity Date")] VerticalsBasedOnSaudaValidityDate = 1,
        [Description("Emails Based On Divisions For Sauda Report")] EmailsBasedOnVerticalsForSaudaReport = 2
    }
}
