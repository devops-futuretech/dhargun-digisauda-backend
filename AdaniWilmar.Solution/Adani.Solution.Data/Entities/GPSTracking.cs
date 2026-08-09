using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class GPSTracking : Auditable
    {
        public long UserId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }  
    }
}
