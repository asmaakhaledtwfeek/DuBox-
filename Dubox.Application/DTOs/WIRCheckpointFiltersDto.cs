namespace Dubox.Application.DTOs
{
    public class WIRCheckpointFiltersDto
    {
        public List<string> StageNumbers { get; set; } = new();
        public List<string> BoxTags { get; set; } = new();
        public List<string> ProjectCodes { get; set; } = new();
        public List<BoxTagWithProject> BoxTagsWithProjects { get; set; } = new();
    }

    public class BoxTagWithProject
    {
        public string BoxTag { get; set; } = string.Empty;
        public string ProjectCode { get; set; } = string.Empty;
    }
}
