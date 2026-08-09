using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class BookedSaudaDto
    {
        public long SaudaId { get; set; }
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public string SaudaNumber { get; set; }
        public bool IsBroker { get; set; }
        public DateTime SaudaBookedDate { get; set; }
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public long SkuCount { get; set; } = 1;
        public int StatusId { get; set; }
        public string Status { get; set; }
        public long SaudaOrderId { get; set; }
        public string Location { get; set; }
        public decimal TotalQuantity { get; set; }


        public string ApprovalUser { get; set; }   // ✅ added
      
        public List<BookedSaudaDetailDto> BookedSaudaDetailDto { get; set; }
        public BookedSaudaDto()
        {
            BookedSaudaDetailDto = new List<BookedSaudaDetailDto>();
        }
    }
    public class BookedSaudaDealerGroupDto
    {
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public List<BookedSaudaDto> BookedSaudaList { get; set; }
        public BookedSaudaDealerGroupDto()
        {
            BookedSaudaList = new List<BookedSaudaDto>();
        }
    }
    public class BookedSaudaDetailDto
    {
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public long SkuCount { get; set; }
    }

    public class BookedSDto
    {
       public long Id { get; set; }
       public long UserId { get; set; }
        public DateTime BiddingDate { get; set; }
        public string SaudaNumber { get; set; }
        public long StatusId { get; set; }
    }

    public class BookedSaudaDetailsDto
    {
        public long DealerId { get; set; }
        public string Dealer { get; set; }

        public List<BookedSaudaDto> BookedSaudaList { get; set; }
        public BookedSaudaDetailsDto()
        {
            BookedSaudaList = new List<BookedSaudaDto>();
        }
    }
}
