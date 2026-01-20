namespace Dubox.Domain.Enums
{
    public enum PanelStatusEnum
    {
        NotStarted = 1,              // Panel created but not manufactured
        Manufacturing = 2,            // Panel being manufactured
        ReadyForDispatch = 3,        // Manufactured, ready to ship
        InTransit = 4,               // On the way to factory (YELLOW)
        ArrivedFactory = 5,          // Arrived at factory (GREEN)
        FirstApprovalPending = 6,    // Waiting for first approval
        FirstApprovalApproved = 7,   // First approval done
        FirstApprovalRejected = 8,   // First approval rejected
        SecondApprovalPending = 9,   // Waiting for second approval
        SecondApprovalApproved = 10, // Second approval done (Ready for installation)
        SecondApprovalRejected = 11, // Second approval rejected
        Installed = 12,              // Panel installed in box
        Rejected = 13,               // Panel rejected (defective)
        
        // Legacy support - map to new statuses
        Yellow = InTransit,          // Backward compatibility
        Green = ArrivedFactory       // Backward compatibility
    }
}
