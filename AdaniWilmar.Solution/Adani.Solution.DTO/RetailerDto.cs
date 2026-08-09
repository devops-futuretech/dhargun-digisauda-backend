using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class RetailerDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }

        public long ZoneId { get; set; }
        public string Zone { get; set; }

        public int StateId { get; set; }
        public string State { get; set; }

        public int TerritoryId { get; set; }
        public string TerritoryName { get; set; }

        public int DistrictId { get; set; }
        public string District { get; set; }     
        
        public int CityId { get; set; }
        public string City { get; set; }

        public string Address { get; set; }
        public string Pincode { get; set; }
        public int AreaId { get; set; }
        public string Area { get; set; }
        public bool IsActive { get; set; }

        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }

        public long FreightZoneId { get; set; }
        public string FreightZoneName { get; set; }
        public long FreightRouteId { get; set; }
        public string FreightRouteName { get; set; }

        public string VisitDay { get; set; }
        public string DistributorSalesMan { get; set; }
        public string DistributorSalesManCode { get; set; }
        public string DistributorCode { get; set; }
        public string DistributorName { get; set; }
        public string ASOASEname { get; set; }
        public string ASOASECode { get; set; }

        public string AccountManager { get; set; }
        public string AccountType { get; set; }
        public string AreaName { get; set; }
        public string OwnersName { get; set; }
        public string DecisionMakerName { get; set; }
        public string DecisionMakerNumber { get; set; }
        public string ChefName { get; set; }
        public string ChefNumber { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public long VerticalId { get; set; }
        public string Vertical { get; set; }
        public string AccountName { get; set; }
        public string SPFZone { get; set; }
        public long DealerId { get; set; }
    }

    public class ActiveRetailerOutputDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long DealerId { get; set; }
        public bool IsLatLonUpdated { get; set; }
    }

    public class RetailerLatLonInputDto
    { 
        //retiler id
        public long CustomerId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
    }
