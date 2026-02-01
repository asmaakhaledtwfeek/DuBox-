namespace Dubox.Application.DTOs
{
    public class TeamPerformanceExportDto
    {
        public object TeamCode { get; set; } = string.Empty;
        public object TeamName { get; set; } = string.Empty;
        public object MembersCount { get; set; } = 0;
        public object TotalAssignedActivities { get; set; } = 0;
        public object Completed { get; set; } = 0;
        public object InProgress { get; set; } = 0;
        public object Pending { get; set; } = 0;
        public object Delayed { get; set; } = 0;
        public object AverageTeamProgress { get; set; } = 0m;
        public object WorkloadLevel { get; set; } = "Normal";
    }
}
