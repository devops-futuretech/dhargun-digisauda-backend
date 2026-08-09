using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DealerOrderDetailsDto
    {
        public string from { get; set; }
        public string to { get; set; }
        public string vehicle_number { get; set; }
        public string driver_name { get; set; }
        public string driver_phone_number { get; set; }
        public string tracking_link { get; set; }
        public VehicleStatusDto status_body { get; set; }
        public string do_number { get; set; }
    }


    public class DealerDetailsParentDto
    {
        public string CurrentStatus { get; set; }
        public string to { get; set; }
        public string vehicle_number { get; set; }
        public string driver_name { get; set; }
        public string driver_phone_number { get; set; }
        public string tracking_link { get; set; }
        public VehicleStatusDto status_body { get; set; }
        public string do_number { get; set; }
    }
}
