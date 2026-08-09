using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum NotificationActions
    {
        [Description("Window Creation")] WindowCreation = 1,
        [Description("Window Price Publish")] WindowPricePublish = 2,
        [Description("Window Stopped")] WindowStopped = 3,
        [Description("Window Completed")] WindowCompleted = 4,
        [Description("Surprise Discount")] SurpriseDiscount = 5,
        [Description("About Window End")] AboutWindowEnd = 6,
        [Description("Customer CounterBid offer")] CustomerCounterBidoffer = 7
    }

    
}
