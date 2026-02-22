namespace Dubox.Application.DTOs
{
    public class QualityIssueFiltersDto
    {
        public List<string> IssueNumbers { get; set; } = new();
        public List<string> BoxTags { get; set; } = new();
        public List<string> ProjectCodes { get; set; } = new();
        public List<BoxTagWithProject> BoxTagsWithProjects { get; set; } = new();
    }
}
