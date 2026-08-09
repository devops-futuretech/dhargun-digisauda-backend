using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class VolumeDiscountSlabDto
    {
        public long VolumeDiscountSlabId { get; set; }
        public long VolumeDiscountGeographyId { get; set; }
        public decimal StartSlabInMT { get; set; }
        public decimal EndSlabInMT { get; set; }
        public decimal DiscountForSlab { get; set; }
    }
}