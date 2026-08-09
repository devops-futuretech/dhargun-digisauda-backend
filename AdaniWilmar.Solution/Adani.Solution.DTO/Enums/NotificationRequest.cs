using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum NotificationRequest
    {
        [Description("Sauda")] Sauda = 1,
        [Description("Indent")] Indent = 2,
        [Description("Counter Bid")] CounterBid = 3,
        [Description("Final Pricing")] FinalPricing = 4,
        [Description("Sauda Limit")] SaudaLimit = 5,
        [Description("Special Rate")] SpecialRate = 6,
        [Description("Sauda Conversion")] SaudaConversion = 7,
        [Description("Sauda Extension")] SaudaExtension = 8,
    }
}
