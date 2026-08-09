using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum NotificationType
    {
        [Description("Sauda Creation")] SaudaCreation = 1,
        [Description("Counter Bid Offer")] CounterBidoffer = 2
    }

    public enum AppNotificationType
    {
        [Description("Email")]
        Email = 1,
        [Description("SMS")]
        SMS = 2,
        [Description("Pushnotification")]
        Pushnotification = 3
    }

    public enum LiveOrTesting
    {
        [Description("AllUser")]
        AllUser = 1,
        [Description("Testing")]
        Testing = 2
    }
}
