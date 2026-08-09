using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Service
{
    public interface IDealerServices
    {
        
    }

    public class DealerService : IDealerServices
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Dealer Service");
        private const string ServiceName = "Dealer Service";
        private string _methodName;

        public DealerService(IAdaniContext salesContext)
        {
            try
            {
                _emamiContext = salesContext;
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Dealer Service", exception);
            }
        }

        
    }
}
