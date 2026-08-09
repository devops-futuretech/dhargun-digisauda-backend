using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class TickerDto  :IAPIInputDTO
    {
        public string EncryptedId { get; set; }
        public long Id { get; set; }
        public string Content { get; set; }
        public string Color { get; set; }
        public bool IsActive { get; set; }
        public TimeSpan FromHours { get; set; }   
        public string FromHoursString { get { return FromHours.ToString(@"dd\.hh\:mm\:ss"); } }
        public string ToHoursString { get { return ToHours.ToString(@"dd\.hh\:mm\:ss");  } } 
        public long LoginUserId { get; set; }
        public TimeSpan ToHours { get; set; }
        public DateTime TickerDate { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
   
}
