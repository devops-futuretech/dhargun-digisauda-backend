using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
   public enum NotificationActionTP
    {
        [Description("Price Release")] PriceRelease = 1,
        [Description("Sauda Creation")] SaudaCreation = 2,
        [Description("Sauda Approval")] SaudaApproval = 3,
        [Description("Sales Order Request Creation")] IndentRequestCreation = 4,
        [Description("Sales Order Request Approval")] IndentRequestApproval = 5,
        [Description("Special Rate Creation")] SpecialRateCreation = 6,
        [Description("Special Rate Approval")] SpecialRateApproval = 7,
        [Description("Limit Enhancement Request Creation")] LimitEnhancementRequestCreation = 8,
        [Description("Limit Enhancement Request Approval")] LimitEnhancementRequestApproval = 9,
        [Description("Sauda Conversion Request")] SaudaConversionRequest = 10,
        [Description("Sauda Extension Request")] SaudaExtensionRequest = 11,
        [Description("Sauda Conversion Approval")] SaudaConversionApproval = 12,
        [Description("Sauda Extension Approval")] SaudaExtensionApproval = 13,
    }
}
