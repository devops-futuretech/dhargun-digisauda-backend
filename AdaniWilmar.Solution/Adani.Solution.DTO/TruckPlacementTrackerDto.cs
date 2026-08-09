using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class TruckPlacementTrackerDto
    {
        public long Id { get; set; }
        public string Plant { get; set; }
        public string PlantOrDepotDesc { get; set; }
        public long AppIndentNo { get; set; }
        public DateTime AppIndentDate { get; set; }
        public DateTime AppIndentTime { get; set; }
        public string InquiryNo { get; set; }
        public DateTime InquiryDate { get; set; }
        public DateTime InquiryTime { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime CreationTime { get; set; }
        public string ContractNumber { get; set; }
        public DateTime ContractValidFromDate { get; set; }
        public string DONo { get; set; }
        public DateTime DOCreationDate { get; set; }
        public DateTime DOCreationTime { get; set; }
        public string Incoterms { get; set; }
        public string VehicleType { get; set; }
        public string VehicleCapacity { get; set; }
        public string TruckIndentNo { get; set; }
        public DateTime TruckReleaseDate { get; set; }
        public DateTime TruckReleaseTime { get; set; }
        public DateTime RevisedTruckIndentReleaseDate { get; set; }
        public DateTime RevisedTruckIndentReleaseTime { get; set; }
        public string DespatchNo { get; set; }
        public DateTime DPCeationDate { get; set; }
        public DateTime DPCeationTime { get; set; }
        public DateTime VehicleReportingDate { get; set; }
        public DateTime VehicleReportingTime { get; set; }
        public DateTime GateInDate { get; set; }
        public DateTime GateInTime { get; set; }
        public DateTime VehicleInDate { get; set; }
        public DateTime VehicleInTime { get; set; }
        public string BillToParty { get; set; }
        public string BillToPartyName { get; set; }
        public string ShipToParty { get; set; }
        public string ShipToPartyName { get; set; }
        public string Destination { get; set; }
        public string City { get; set; }
        public string DestinationState { get; set; }
        public string DestStateDescription { get; set; }
        public string SKUCode { get; set; }
        public string SKUDescription { get; set; }
        public string PrimaryTransporterCode { get; set; }
        public string PrimaryTransporterName { get; set; }
        public string PrimaryTransporterVehicleNumber { get; set; }
        public DateTime PrimaryTransporterIndentDate { get; set; }
        public DateTime PrimaryTransporterIndentTime { get; set; }
        public string DoCreationHeaderStatus { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceTime { get; set; }
        public DateTime VehicleOutDate { get; set; }
        public DateTime VehicleOutTime { get; set; }
    }

    public class TruckPlacementTrackerList
    {
        public List<TruckPlacementTrackerDto> TruckPlacementTracker { get; set; }
        public TruckPlacementTrackerList()
        {
            TruckPlacementTracker = new List<TruckPlacementTrackerDto>();
        }
    }
}
