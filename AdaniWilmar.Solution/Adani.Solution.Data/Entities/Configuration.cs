using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class Configuration : EntityLong
    {
        public string Name { get; set; }
        [MaxLength(250)]
        public string Key { get; set; }
        [MaxLength(250)]
        public string Value { get; set; }
        public bool Isactive { get; set; }
        public int TypeId { get; set; }
        //public int SaudaBookingTypeId { get; set; }
    }
}
