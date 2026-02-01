namespace Dubox.Domain.Enums
{
    public enum PanelStatusEnum
    {
        NotStarted = 1,  
        InProgress = 2,              // Workflow: Panel being worked on at site
        Completed = 3,               // Workflow: Panel completed at site, ready to move
        OnHold = 4,                  // Panel on hold, has quality issue
        Rejected = 5,                // Panel rejected, has open quality issue
        FirstApprovalPending = 6,    // Waiting for first approval
        FirstApprovalApproved = 7,   // First approval passed
        SecondApprovalPending = 8,   // Waiting for second approval
        SecondApprovalApproved = 9,  // Second approval passed
        SecondApprovalRejected = 10, // Second approval rejected
    }
}
