using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class ConfigurationForDivisionsAndEmails : EntityLong
    {
        public string Name { get; set; }
        [MaxLength(250)]
        public string Key { get; set; }
        public string Value { get; set; }
        public bool Isactive { get; set; }
        public int TypeId { get; set; }
        public int SaudaBookingTypeId { get; set; }
    }
}
