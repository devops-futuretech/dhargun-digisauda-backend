using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class LoginUserIdDto : VerticalIdDto
    {
        public DataSourceRequest DataSourceRequest { get; set; }
        public long DealerId { get; set; }
        public List<long> NationalHeadIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
        public List<long> BdoIds { get; set; }
        public List<long> DealerIds { get; set; }
        public List<string> DealerCodes { get; set; }
        public long LoginUserId { get; set; }
        public long RoleId { get; set; }
        public bool IsToReturnInactiveData { get; set; }
        public long IsReturnInactiveData { get; set; }
        public long DueStatus { get; set; }
        public int StateId { get; set; }
        public int PlantId { get; set; }
        public int DepotId { get; set; }
        public int IntercomId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public long OrganizationReportingToId { get; set; }
        public string DealerCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string Name { get; set; }
        public int PageNo { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public List<long> DivisionIds { get; set; }
        public DateTime Date { get; set; }
        public bool IsRequestFromWeb { get; set; }
        public bool IsPendingSauda { get; set; }
        public bool IsSaudaConfig { get; set; }
        public bool IsComplete { get; set; }
        //public List<long> SalesOrganizationIds { get; set; }
        //public List<long> DistributionChannelIds { get; set; }
        public string ZoneIds { get; set; }
        public string StateIds { get; set; }
        public string DistrictIds { get; set; }
        public string CityIds { get; set; }
        public string Status { get; set; }
    }

    public class BookedSaudaInputDto
    {
        public long LoginUserId { get; set; }
        public long DealerId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }

    }

    public class VerticalIdDto
    {
        public long VerticalId { get; set; }
        public long DistributionId { get; set; }
    }

    public class KendoGridResult : LoginUserIdDto
    {
        public long Id { get; set; }
        public DataSourceRequest DataSourceRequest { get; set; }
    }

    public class KendoDataSourceResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Total { get; set; }
        public IEnumerable<AggregateResult> AggregateResults { get; set; }
        public object Errors { get; set; }
    }
}
