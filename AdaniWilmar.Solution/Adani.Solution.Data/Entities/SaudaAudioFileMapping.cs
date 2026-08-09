using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
     public class SaudaAudioFileMapping : Auditable
    {
        public long UserId { get; set; }
        public long SaudaId { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public long? AudioFileDetailsForActiveCustomersId { get; set; }
        public int MediaTypeId { get; set; }
        public string ImagePath { get; set; }
        public virtual AudioFileDetailsForActiveCustomers AudioFileDetailsForActiveCustomers { get; set; }
    }
}
