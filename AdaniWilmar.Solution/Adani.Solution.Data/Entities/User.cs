using Adani.Solution.Data.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class User : Auditable
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string Email { get; set; }
        [MaxLength(40)]
        public string MobileNumber { get; set; }
        [MaxLength(250)]
        public string AdditionalMobileNumber { get; set; }
        [MaxLength(250)]
        public string Password { get; set; }
        [MaxLength(10)]
        public string OtpNumber { get; set; }
        //public long? RoleId { get; set; }
        //public string UserCode { get; set; }
        [MaxLength(1000)]
        public string PushTokenKey { get; set; }

        public long? ReportingToId { get; set; }
        //public long? SpecialityFatReportingToId { get; set; }
        //public long? CMSReportingToId { get; set; }
        //public long? FreightZoneId { get; set; }
        //public long? FreightRouteId { get; set; }

        public string Remarks { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? LastLoggedInDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? PreviousLoggedInDate { get; set; }
        //public bool IsApproved { get; set; } = false;
        public long? ApprovedBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? ApprovedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsActiveForCall { get; set; }
        public bool IsBlacklisted { get; set; }
        [MaxLength(1000)]
        public string ImageUrl { get; set; }
        public long? ParentUserId { get; set; }
        public int? RegistrationTypeId { get; set; }
        [MaxLength(500)]
        public string Region { get; set; }
        [MaxLength(10)]
        public string Pincode { get; set; }
        [MaxLength(500)]
        public string Street { get; set; }

        public long? ZoneId { get; set; }
        public int DistrictId { get; set; }
        [MaxLength(500)]
        public string District { get; set; }

        public int CityId { get; set; }
        [MaxLength(500)]
        public string City { get; set; }

        public int StateId { get; set; }
        [MaxLength(500)]
        public string State { get; set; }

        public int TerritoryId { get; set; }
        [MaxLength(500)]
        public string Territory { get; set; }

        [MaxLength(250)]
        public string ExecutivePassword { get; set; }
        [MaxLength(50)]
        public string McsNo { get; set; }

        //Broker And Dealer
        [MaxLength(4000)]
        public string Code { get; set; }
        //[MaxLength(20)]
        //public string MobileNumber1 { get; set; }
        [MaxLength(20)]
        public string MobileNumber2 { get; set; }
        //[MaxLength(4000)]
        //public string AddressLine1 { get; set; }
        //[MaxLength(4000)]
        //public string AddressLine2 { get; set; }
        //[MaxLength(4000)]
        //public string AddressLine3 { get; set; }
        public string GSTN { get; set; }
        public string VisitDay { get; set; }
        public int? SaudaValidityPeriod { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal SaudaLimit { get; set; }

        public string WeeklyClosingDay { get; set; }
        public string MonthlyPotential { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal Loadability { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal DepotLoadability { get; set; }

        [MaxLength(4000)]
        public string Address1 { get; set; }
        [MaxLength(4000)]
        public string Address2 { get; set; }
        public string CustClass { get; set; }

        //Employee
        public string Branch { get; set; }
        public string SalesAccess { get; set; }
        [MaxLength(150)]
        public string Designation { get; set; }
        public long? HeadquartersId { get; set; }       
        
        public string Acedns { get; set; }
        public long? SaudaBookingTypeId { get; set; }
        public long? IncoTermsId { get; set; }
        public long? TransportModeId { get; set; }
        //public long? DivisionId { get; set; }

        public bool IsSelf { get; set; }
        public bool IsBroker { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime PasswordModifiedDate { get; set; }

        //public virtual SaudaBookingType SaudaBookingType { get; set; }        
        public virtual IncoTerms IncoTerms { get; set; }        

        //SAP data        
        public string ADRNR { get; set; }
        //public string DeliveringPlant { get; set; }
        public string CentralDeletionFlag { get; set; }
        //public bool IsSAPData { get; set; }
        //public bool IsSAPDataSyncOrNot { get; set; }
        //public int SapStatusId { get; set; }
        public string CustomerGroup { get; set; }
        public string FSSAINumber { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string InActiveRemarks { get; set; }
        //public long CustomerGroupOneId { get; set; }
        //public long CustomerGroupTwoId { get; set; }
        public long CustomerGroupFiveId { get; set; }
        [ForeignKey("InActiveRemark")]
        public long? InActiveRemarkId { get; set; }
        [MaxLength(250)]
        public string ContactPersonName { get; set; }
        public string CompanyCode { get; set; }
        public string DepartmentName { get; set; }
        public string DirectManagerEmployee { get; set; }
        public string OfficeCountry { get; set; }
        public string ShipToPartyCode { get; set; }
        public string ProfilePath { get; set; }

        //public virtual Zone Zone { get; set; }
        //public virtual Division Division { get; set; }
        //public virtual FreightZone FreightZone { get; set; }
        //public virtual FreightRoute FreightRoute { get; set; }
        //public virtual Headquarters Headquarters { get; set; }
        public virtual DeleteListCreation InActiveRemark { get; set; }
        public string LineId { get; set; }
        [MaxLength(10)]
        public string TANNumber { get; set; }
    }
}
