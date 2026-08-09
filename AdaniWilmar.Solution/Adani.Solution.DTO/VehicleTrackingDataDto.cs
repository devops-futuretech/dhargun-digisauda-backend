using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class VehicleTrackingDataDto
    {      
       public string dealer_code { get; set; }
       public string dealer_name { get; set; }
       public List<DealerOrderDetailsDto> do_details { get; set; }
    }

    public class VehicleTrackingLoginDto
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class Data
    {
        public bool exists { get; set; }
        public string token { get; set; }
    }

    public class VehicleTrackingLoginResponseDto
    {
        public int status { get; set; }
        public string message { get; set; }
        public long system_time { get; set; }
        public Data data { get; set; }
    }


    public class DONumberddlDto
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public string BillingNo { get; set; }
        public DateTime BillingDate { get; set; }
    }

}
