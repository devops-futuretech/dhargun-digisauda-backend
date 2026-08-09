using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum Device
    {
       
        [Description("All")] All = 1,
        [Description("Portal")] Portal = 2,
        [Description("Dealer App")] DealerApp = 3,
        [Description("Manager App")] ManagerApp = 4,
        [Description("Sales App")] SalesApp = 5,

    }
}
