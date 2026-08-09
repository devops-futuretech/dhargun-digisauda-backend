using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class TruckPlacementTrackerAPPDto
    {
        public int ListCount { get; set; }

        public List<TruckPlacementTrackerListDto> TruckPlacementTrackerList { get; set; }

        public TruckPlacementTrackerAPPDto()
        {
            TruckPlacementTrackerList = new List<TruckPlacementTrackerListDto>();
        }
    }

    public class TruckPlacementTrackerListDto
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
        public string DONo { get; set; }
        public DateTime DOCreationDate { get; set; }
        public DateTime DOCreationTime { get; set; }
        public string VehicleCapacity { get; set; }
        public DateTime GateInDate { get; set; }
        public DateTime GateInTime { get; set; }
        public string PrimaryTransporterVehicleNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceTime { get; set; }
    }
}
