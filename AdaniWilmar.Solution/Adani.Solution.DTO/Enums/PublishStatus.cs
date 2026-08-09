using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum PublishStatus
    {
        [Description("Started")] Started = 1,
        [Description("Completed")] Completed = 2,
        [Description("Failed")] Failed = 3,
    }

    public enum PricePublishStatus
    {
        [Description("Pending")] Pending = 1,
        [Description("Started")] Started = 2,
        [Description("Completed")] Completed = 3,
        [Description("CompletedWithError")] CompletedWithError = 4,
        [Description("Failed")] Failed = 5
    }

    public enum PublishButtonStatus
    {
        [Description("Price Generating")] PriceGenerating = 1,
        [Description("Publish")] Publish = 2,
        [Description("Published")] Published = 3,
        [Description("PriceGenerateFailed")] PriceGenerateFailed = 4
    }

    public enum FinalPriceTaskStatus
    {
        [Description("Created")] Created = 1,
        [Description("WaitingForActivation")] WaitingForActivation = 2,
        [Description("WaitingToRun")] WaitingToRun = 3,
        [Description("Running")] Running = 4,
        [Description("WaitingForChildrenToComplete")] WaitingForChildrenToComplete = 5,
        [Description("RanToCompletion")] RanToCompletion = 6,
        [Description("Canceled")] Canceled = 7,
        [Description("Faulted")] Faulted = 8
    }

    public enum ExeStatus
    {
        [Description("Pending")] Pending = 1,
        [Description("Started")] Started = 2,
        [Description("Completed")] Completed = 3
    }
}
