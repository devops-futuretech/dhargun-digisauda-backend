using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class CrossAndUpsellConfigurationDto
    {
        public long Id { get; set; } = 0;
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public List<long> ZoneId { get; set; }
        public List<long> StateId { get; set; }
        public List<long> OilTypeId { get; set; }
        public long PackGroupId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool LineItemIsActive { get; set; }
        public string EncryptedId { get; set; }
        public long LoginUserId { get; set; }
        public List<SaudaConditionalBookingSkuDto> SkuBookingCombinationList { get; set; }
    }

    public class SuadaConditionalBookingInputDto
    {
        public long Id { get; set; }
        public long LoginUserId { get; set; }
        public string EncryptedId { get; set; }
    }

    public class SuadaConditionalBookingSkusInputDto
    {
        public List<SkuInfoDto> Skus { get; set; }
        public long PlantId { get; set; }
        public long DealerId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public long LoginUserId { get; set; }
    }

    public class SkuInfoDto
    {
        public long SkuId { get; set; }
        public decimal Quantity { get; set; }
        public long OilTypeId { get; set; }
    }
}
