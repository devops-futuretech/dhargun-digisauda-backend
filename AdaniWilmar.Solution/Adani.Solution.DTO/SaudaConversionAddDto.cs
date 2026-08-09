using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SaudaConversionAddDto:LoginUserIdDto
    {
        public long SaudaId { get; set; }
        public IList<SaudaConversionOrderAddDto> SaudaConversionOrders { get; set; }
        public SaudaConversionAddDto()
        {
            SaudaConversionOrders = new List<SaudaConversionOrderAddDto>();
        }
    }
}
