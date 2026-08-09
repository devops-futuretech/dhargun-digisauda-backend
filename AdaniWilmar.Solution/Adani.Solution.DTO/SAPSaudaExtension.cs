using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class HANASaudaExtension
    {
        public List<HANASaudaExtensionDto> SaudaExtensionList { get; set; }

        public HANASaudaExtension()
        {
            SaudaExtensionList = new List<HANASaudaExtensionDto>();
        }
    }
    public class HANASaudaExtensionDto
    {
        public string SaudaNumber { get; set; }
        public string Remarks { get; set; }
        public bool Status { get; set; }
    }
    public class SAPSaudaExtension
    {
        public long SaudaExtensionDetailsApprovalId { get; set; }
        public string RequestDate { get; set; }
        public DateTime ExtensionDate { get; set; }
        public string SaudaNumber { get; set; }
        public string Remarks { get; set; }
        public bool Status { get; set; }
    }

    public class HANASaudaExtensionList
    {
        public List<HANASaudaExtensionDetails> Header { get; set; }
        public HANASaudaExtensionList()
        {
            Header = new List<HANASaudaExtensionDetails>();
        }
    }

    public class HANASaudaExtensionDetails
    {
        public DateTime ExtensionDate { get; set; }
        public string SaudaNumber { get; set; }
    }

    public class HANASaudaExtensionAPPToSAP
    {
        public DateTime ExtensionDate { get; set; }
        public long ExtensionDays { get; set; }
        public List<string> SaudaNumbers { get; set; }
    }

    public class HANASaudaCommonFunction
    {
        public List<HANASaudaCommonFunctionList> Header { get; set; }
        public HANASaudaCommonFunction()
        {
            Header = new List<HANASaudaCommonFunctionList>();
        }
    }

    public class HANASaudaCommonFunctionList
    {
        public string Flag { get; set; }
        public string Flag_Description { get; set; }
        public long Impiger_Request_No { get; set; }
        public string SAP_Document_No { get; set; }
        public string SAPRefDoc { get; set; }
        public string DueDate { get; set; }
        public string Quantity { get; set; }
        public string Material { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}
