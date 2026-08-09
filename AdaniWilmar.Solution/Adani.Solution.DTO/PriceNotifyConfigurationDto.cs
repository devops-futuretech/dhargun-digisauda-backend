using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class PriceNotifyConfigurationDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }

        public List<long> IncoTermId { get; set; }
        public IList<long> ZoneId { get; set; }
        public List<long> StateId { get; set; }
        public List<long> TerritoryId { get; set; }
        public List<long> CityId { get; set; }
        public List<long> SkuId { get; set; }
        
        public string IncoTerms { get; set; }
        public bool HasChildren { get; set; }

        public List<long> CityIds { get; set; }
        public string CityIdstr { get; set; }

        public bool IsSMS { get; set; }
        public bool IsEmail { get; set; }
        public bool IsPushNotification { get; set; }
        public DateTime NotificationDate { get; set; }

        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long VerticalId { get; set; }
        public long? OilTypeId { get; set; }

        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public List<CityDetails> Cities { get; set; }
    }
}
