using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class CustomerGroupDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long VerticalId { get; set; }
        public string Vertical { get; set; }
        public bool IsActive { get; set; }
        public bool IsBaseGroup { get; set; }
        public string CreatedBy { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }

        public string SelecteDealerIdsString { get; set; }
        public List<long> SelectedDealerIds { get; set; }
        public List<long> SelectedCustomerIdsToRemove { get; set; }
        public string SelectedCustomerIdsToRemoveString { get; set; }

        public List<CustomerGroupDetailDto> CustomerGroupDetailDtoList { get; set; }

        public CustomerGroupDto()
        {
            CustomerGroupDetailDtoList = new List<CustomerGroupDetailDto>();
        }
    }

    public class BdoBiddingWindowDetailsDto
    {
        public BdoBiddingWindowDetailsDto()
        {
            this.DealerDetails = new List<DealerDetailsDto>();
        }
        public long BiddingWindowId { get; set; }
        public string BiddingWindowName { get; set; }
        public long CustomerGroupId { get; set; }
        public string CustomerGroupName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string StartEndTime { get; set; }
        public string WindowStatus { get; set; }
        public long WindowStatusId { get; set; }
        public DateTime ServerDateTime { get; set; }
        public int UsersCount { get; set; }

        public List<DealerDetailsDto> DealerDetails { get; set; }
    }

    public class DealerBiddingWindowDetailsDto
    {
        public long BiddingWindowId { get; set; }
        public string BiddingWindowName { get; set; }
        public long CustomerGroupId { get; set; }
        public string CustomerGroupName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string StartEndTime { get; set; }
        public string WindowStatus { get; set; }
        public long WindowStatusId { get; set; }
        public DateTime ServerDateTime { get; set; }
    }

    public class DealerDetailsDto
    {
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
    }
    public class CustomerGroupFiveDto: IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public long LoginUserId { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public bool IsActive { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }

    }
    public class CustomerGroupFiveddlDto
    {
        public long CustomerGroupId { get; set; }
        public string CustomerGroupName { get; set; }


    }
    public class CustomerGroupFiveExportDto
    {
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public bool IsActive { get; set; }
    }
    public class CustomerGroupOneDto
    {
        public long Id { get; set; }
        public long LoginUserId { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public bool IsActive { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }

    }
    public class CustomerGroupOneandTwoddlDto
    {
        public long CustomerGroupId { get; set; }
        public string CustomerGroupName { get; set; }

        
    }

    public class CustomerGroupMappingDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long CustomerGroupId { get; set; }
        public List<long> derivedCustomerGroupId { get; set; }
        public string CustomerGroupName { get; set; }
        public string DerivedCustomerGroupName { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class CustomerGroupMappingGridDto 
    {
        public long derivedCustomerGroupId { get; set; }
        public string DerivedCustomerGroupName { get; set; }
    }
    public class CustomerGrouOneTwoExportDto
    {
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public bool IsActive { get; set; }
    }

    public class CustomerGroupOneAndTwoUploadDto : CommonResultDto
    {
        public long LoginUserId { get; set; }
        public string CustomerGroupName { get; set; }
        public string CustomerGroupCode { get; set; }
        public string IsActive { get; set; }
       
    }
    public class CustomerGroupFiveUploadDto : CommonResultDto
    {
        public long LoginUserId { get; set; }
        public string CustomerGroupName { get; set; }
        public string CustomerGroupCode { get; set; }
        public string IsActive { get; set; }

    }

}
