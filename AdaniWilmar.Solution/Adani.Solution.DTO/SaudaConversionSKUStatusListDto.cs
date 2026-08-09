using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SaudaConversionSKUStatusListDto
    {
        public SaudaConversionSkuStatusWiseDetailsDto PendingSaudaConversionList { get; set; }
        public SaudaConversionSkuStatusWiseDetailsDto ApprovedSaudaConversionList { get; set; }        
    }

    public class SaudaConversionSkuStatusWiseDetailsDto
    {
        //ZonalTrader List
        public List<SaudaConversionSkuZonalHeadUserList> ZonalHeadUserList { get; set; }
        //StateTrader List
        public List<SaudaConversionSkuBDOUserList> BDOUsersList { get; set; }
        //Dealers List
        public List<SaudaConversionSkuDealerUserList> BDODealerUsersList { get; set; }
        //Sauda details
        public List<SaudaConversionSkusDetail> SaudaConversionSkuDetails { get; set; }
        public SaudaConversionSkuStatusWiseDetailsDto()
        {
            ZonalHeadUserList = new List<SaudaConversionSkuZonalHeadUserList>();
            BDOUsersList = new List<SaudaConversionSkuBDOUserList>();
            BDODealerUsersList = new List<SaudaConversionSkuDealerUserList>();
            SaudaConversionSkuDetails = new List<SaudaConversionSkusDetail>();
        }
    }

    public class SaudaConversionSkuZonalHeadUserList
    {
        public long ZonalHeadId { get; set; }
        public string ZonalHeadName { get; set; }
        public string ZonalHeadAddress { get; set; }

    }

    public class SaudaConversionSkuBDOUserList
    {
        //StateTrader details        
        public long BDOId { get; set; }
        public string BDOName { get; set; }
        public string BDOAddress { get; set; }
        //StateTrader - Dealer List
        public List<SaudaConversionSkuDealerUserList> BDODealerUsersList { get; set; }
        public SaudaConversionSkuBDOUserList()
        {
            BDODealerUsersList = new List<SaudaConversionSkuDealerUserList>();
        }
    }

    public class SaudaConversionSkuDealerUserList
    {
        //Dealer details
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public string DealerAddress { get; set; }
        //Conversion details
        public List<SaudaConversionSkusDetail> SaudaConversionSkuDetails { get; set; }
        public SaudaConversionSkuDealerUserList()
        {
            SaudaConversionSkuDetails = new List<SaudaConversionSkusDetail>();
        }
        
    }

    public class SaudaConversionSkusDetail
    {
        //Conversion details
        public long SkuConversionId { get; set; }
        public long? SkuConversionHeaderId { get; set; }
        public string SkuName { get; set; }
        public DateTime ConversionCreatedDate { get; set; }
        public DateTime ConversionModifiedDate { get; set; }
        public string SaudaNumber { get; set; }
        public decimal SaudaQuantityInMT { get; set; }     
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public string BdoName { get; set; }
        public string ZonalHeadName { get; set; }
        public string Remarks { get; set; }
        public decimal SaudaQuantityInSku { get; set; }
        public string PlantOrDepotCode { get; set; }
        public string PlantOrDepotName { get; set; }
        public long PlantId { get; set; }
        public long DepotId { get; set; }
        public string ConversionCreatedDateInstring { get; set; }
        public bool ReprocessStatus { get; set; }
        public bool SaudaConversionUpdateFromSap { get; set; }
        public bool IsSapDataSync { get; set; }
        public long StatusId { get; set; }
    }

    public class SaudaConversionSkusDetailExportDto
    {
        public long SkuConversionId { get; set; }
        public string Dealer { get; set; }
        public string StateTrader { get; set; }
        public string ZonalTrader { get; set; }
        public string SkuName { get; set; }
        public string PlantOrDepotCode { get; set; }
        public string PlantOrDepotName { get; set; }
        public string ConversionCreatedDate { get; set; }
        public string ConversionQuantityInMT { get; set; }
        public string ConversionQuantityInCase { get; set; }
        public string Remarks { get; set; }
        
     }

    public class SaudaConversionPantAndDepotDetails
    {
        public long PlantOrDepotId { get; set; }
        public string PlantOrDepotCode { get; set; }
        public string PlantOrDepotName { get; set; }
    }

    }
