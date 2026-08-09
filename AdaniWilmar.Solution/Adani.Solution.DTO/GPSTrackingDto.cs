using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class GPSTrackingDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public long LoginUserId { get; set; }
    }
}
