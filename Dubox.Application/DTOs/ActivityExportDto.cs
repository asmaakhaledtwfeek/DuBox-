namespace Dubox.Application.DTOs
{
    public class ActivityExportDto
    {
        public object ActivityId { get; set; } = string.Empty;
        public object ActivityName { get; set; } = string.Empty;
        public object BoxTag { get; set; } = string.Empty;
        public object ProjectName { get; set; } = string.Empty;
        public object AssignedTeam { get; set; } = string.Empty;
        public object Status { get; set; } = string.Empty;
        public object ProgressPercentage { get; set; } = string.Empty;
        public object PlannedStartDate { get; set; } = string.Empty;
        public object PlannedEndDate { get; set; } = string.Empty;
        public object ActualStartDate { get; set; } = string.Empty;
        public object ActualEndDate { get; set; } = string.Empty;
        public object ActualDuration { get; set; } = string.Empty;
        public object DelayDays { get; set; } = string.Empty;
    }
}
