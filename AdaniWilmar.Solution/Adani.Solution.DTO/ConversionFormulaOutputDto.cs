using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ConversionFormulaOutputDto
    {
        public string OilType { get; set; }
        public string DerivedSku { get; set; }
        public string Formula { get; set; }
        //public List<OilTypeFormulaDetailDto> OilTypeFormulaDetails { get; set; }

        //public ConversionFormulaOutputDto()
        //{
        //    OilTypeFormulaDetails = new List<OilTypeFormulaDetailDto>();
        //}
    }
    public class OilTypeFormulaDetailDto
    {
        public string DerivedSku { get; set; }
        public string Formula { get; set; }
    }
}
