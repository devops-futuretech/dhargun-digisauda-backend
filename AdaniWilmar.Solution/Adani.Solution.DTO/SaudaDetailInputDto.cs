using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Adani.Solution.DTO
{
    public class SaudaDetailInputDto : UserIdDto
    {
        public DateTime BiddingDate { get; set; }
        public List<long> SkuIds  { get; set; }
        public long SaudaId { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }

    }

    public class SkuwithDealerFilterInputDto : LoginUserIdDto
    {
        public DateTime BiddingDate { get; set; }
        public List<long> DealerIds { get; set; }
        public List<long> SkuIds { get; set; }
        public List<long> BdoIds { get; set; }
    }

    public class SkuwithDealerOutputDto 
    {
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public List<DropDownDto> SkuList { get; set; }
        public  SkuwithDealerOutputDto()
        {
            SkuList = new List<DropDownDto>();
        }
    }

    public class CreditLimitAndCreditExposureInputDto : LoginUserIdDto
    {
        public long CreditId { get; set; }
        //public List<long> DealerIds { get; set; }
        public List<long> BdoIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
        public List<long> NationalHeadIds { get; set; }
    }

    public class CreditLimitAndCreditExposureOutputDto 
    {
        public string DealerCode { get; set; }
        public string DealerName  { get; set; }
        public string CreditAccountNumber { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CreditExposure { get; set; }
        public decimal AvailableCreditLimit { get; set; }
        public decimal SalesValue { get; set; }
        public decimal TotalReceivable { get; set; }
        public decimal GrossExposure { get; set; }
        public decimal OpenExposure { get; set; }      
    }

    public class ContactListForActiveCallInputDto : LoginUserIdDto
    {
        public long ZHId { get; set; }
        public List<long> BdoIds { get; set; }
        public string CallRecordedFileName { get; set; }
        public long BdoId { get; set; }
        public List<AudioFileWithUserDetails> AudioFileDetailIds { get; set; }
        public long SaudaId { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public List<string> ImagePaths { get; set; }
        public long BrokerId { get; set; }
        public  ContactListForActiveCallInputDto()
        {
            AudioFileDetailIds = new List<AudioFileWithUserDetails>();
        }
    }

    public class AudioFileWithUserDetails
    {
        public long UserId { get; set; }
        public long AudioFileDetailId { get; set; }
    }

        public class ContactListForActiveCallOutputDto
    {
        public long DealerId { get; set; }
        public long BdoId { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string MobileNumber { get; set; }
        public string AdditionalMobileNumber { get; set; }
        public string BdoName { get; set; }
        public string BdoCode { get; set; }
        public string CallRecordedFileName { get; set; }
        public long AudioFileDetailId { get; set; }
        public string ContactPersonName { get; set; }
        public string AudioFileNameInServerPath { get; set; }
        public string BrokerOrDealer { get; set; }
        public int CallDuration { get; set; }
    }

    public class CallRecordingInputDto : LoginUserIdDto
    {
        public string DialerMobileNumber { get; set; }
        public string ReceiverMobileNumber { get; set; }
        public long DialerId { get; set; }
        public long ReceiverId { get; set; }
        public string CallRecordedFileName { get; set; }
        public int CallDuation { get; set; }
        public string CallStartTime { get; set; }
        public HttpPostedFile file { get; set; }
    }

    public class CallRecordingGetInputDto
    {
        public string DialerMobileNumber { get; set; }
        public long VerticalId { get; set; }
        public long DealerId { get; set; }
    }
}
