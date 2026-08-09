using System;
using System.Collections.Generic;


namespace Adani.Solution.Data.Entities
{
    public class PriceNotifyConfiguration : Auditable
    {
        public string IncoTermId { get; set; }
        public string ZoneId { get; set; }
        public string StateId { get; set; }
        public string TerritoryId { get; set; }
        public string CityId { get; set; }
        public string SkuId { get; set; }

        public bool IsSMS { get; set; }
        public bool IsEmail { get; set; }
        public bool IsPushNotification { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}
