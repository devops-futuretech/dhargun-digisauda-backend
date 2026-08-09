using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SupportAddInputDto : LoginUserIdDto
    {
        public string Description { get; set; }
        public int ComponentId { get; set; }
        public int ImpactId { get; set; }
        public int FeatureId { get; set; }
        public string Feature { get; set; }

        public List<string> Attachments { get; set; }
    }
}
