using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class VehicleStatusDto
    {
        public string current_status { get; set; }
        public string status { get; set; }
        public string last_known_location { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public string last_known_date_time { get; set; }
        public int distance_covered { get; set; }
        public string eta { get; set; }
        public string arrived_date_time { get; set; }
        public string trip_closed_date_time { get; set; }
        public string DoNumber { get; set; }
        public string BillingNumber { get; set; }
        public string VehicleNumber { get; set; }
        public string dealer_code { get; set; }
        public string dealer_name { get; set; }
    }
    public class VehicleInputDto
    {
        public string CustomerCodeList { get; set; }
        public string DoNumberList { get; set; }
        public string LiftingList { get; set; }
        public string BillingNoList { get; set; }
    }

    public class DataResponse
    {
        public string dealer_code { get; set; }
        public string dealer_name { get; set; }
        public List<DoDetail> do_details { get; set; }
    }

    public class DoDetail
    {
        public int status_code { get; set; }
        public string message { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string vehicle_number { get; set; }
        public string driver_name { get; set; }
        public string driver_phone_number { get; set; }
        public string tracking_link { get; set; }
        public StatusBody status_body { get; set; }
        public string do_number { get; set; }
    }

    public class VehicleTrackingDto
    {
        public string message { get; set; }
        public int status { get; set; }
        public DataResponse data { get; set; }
    }

    public class StatusBody
    {
        public string current_status { get; set; }
        public string status { get; set; }
        public string last_known_location { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public string last_known_date_time { get; set; }
        public int distance_covered { get; set; }
        public string eta { get; set; }
        public string arrived_date_time { get; set; }
        public string trip_closed_date_time { get; set; }
    }

    public class LiftingSkuInputDto
    {
        public long LiftingId { get; set; }
        public string DoNumber { get; set; }
        public bool IsLiftingId { get; set; }
        public long LoginUserId { get; set; }
        public List<DONumberDto> DoNumbers { get; set; }
        public LiftingSkuInputDto()
        {
            DoNumbers = new List<DONumberDto>();
        }
    }
    public class DONumberDto
    {
        public string DoNumber { get; set; }
        public string BillingNo { get; set; }
        public long LiftingId { get; set; }
        public string Status { get; set; }
    }
    public class LiftingUpdateDto
    {
        public long LoginUserId { get; set; }
        public List<string> DoNumbers { get; set; }
    }
    public class TrackingViewDto
    {
        public string DealerName { get; set; }
        public string DealerCode { get; set; }
        public string ShipToParty { get; set; }
        public List<TrackSkuDto> SkuList { get; set; }
        public DoDetail DeliveryDetail { get; set; }
        public TrackingViewDto()
        {
            SkuList = new List<TrackSkuDto>();
        }
    }
    public class TrackSkuOutputDto
    {
        public string DoNumber { get; set; }
        public string BillingNumber { get; set; }
        public string ShipToParty { get; set; }
        public DateTime BillingDate { get; set; }
        public long LiftingRequestId { get; set; }
        public List<TrackSkuDto> Materials { get; set; }
        public TrackSkuOutputDto()
        {
            Materials = new List<TrackSkuDto>();
        }
    }
    public class TrackSkuDto
    {
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityCase { get; set; }
        public string OilTypeName { get; set; }
        public string ShipToParty { get; set; }
    }
}
