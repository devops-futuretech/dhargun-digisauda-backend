using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum Status
    {
        [Description("Pending")] Pending = 1,
        [Description("Approved")] Approved = 2,
        [Description("Rejected")] Rejected = 3,
        [Description("Revised")] Revised = 4,
        [Description("Hold")] Hold = 5,
        [Description("Completed")] Completed = 6,
        [Description("WaitingForApproval")] WaitingForApproval = 7,
        [Description("Processed")] Processed = 8,
        [Description("RequestForApproval")] RequestForApproval = 9,
        [Description("RequestForApproval")] RequestForApproval2 = 10,
        [Description("WaitingForConfirmation")] WaitingForConfirmation = 11,
        [Description("Requested")] Requested = 12,
        [Description("Inprogress")] Inprogress = 13,
        [Description("Deleted")] Deleted = 14,
        [Description("WaitingForRequestApproval")] WaitingForRequestApproval = 15,
    }
}
