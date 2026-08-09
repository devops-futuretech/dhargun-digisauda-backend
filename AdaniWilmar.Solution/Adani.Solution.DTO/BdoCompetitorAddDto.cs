using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class BdoCompetitorAddDto
    {
        public string Name { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public int UserType { get; set; }
        public long DealerId { get; set; }
        public long CreatedBy { get; set; }
        public List<BdoCompetitorSkuDto> BdoCompetitorSkuDetails { get; set; }
        public List<FileListDto> FileList { get; set; }
        public BdoCompetitorAddDto()
        {
            BdoCompetitorSkuDetails = new List<BdoCompetitorSkuDto>();
            FileList = new List<FileListDto>();
        }
    }
    public class FileListDto
    {
        public long Id { get; set; }
        public string FileExtension { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
