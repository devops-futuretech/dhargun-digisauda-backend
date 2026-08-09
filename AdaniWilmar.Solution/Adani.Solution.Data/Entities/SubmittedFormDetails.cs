using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SubmittedFormDetails : Auditable
    {
        public long SubmittedFormId { get; set; }
        public long SkuId { get; set; }
        public long PlantId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }

        public virtual SubmittedForm SubmittedForm { get; set; }
        public virtual Sku Sku { get; set; }
        public virtual State State { get; set; }
        public virtual City City { get; set; }
    }
}
