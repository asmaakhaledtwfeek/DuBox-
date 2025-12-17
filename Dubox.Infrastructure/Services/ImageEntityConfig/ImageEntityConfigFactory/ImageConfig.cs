using Dubox.Domain.Services.ImageEntityConfig;

namespace Dubox.Infrastructure.Services.ImageEntityConfig.ImageEntityConfigFactory
{
    public class ProgressUpdateImageConfig : IImageEntityConfig
    {
        public string ForeignKeyName => "ProgressUpdateId";
    }

    public class QualityIssueImageConfig : IImageEntityConfig
    {
        public string ForeignKeyName => "IssueId";
    }
    public class WIRCheckpointImageConfig : IImageEntityConfig
    {
        public string ForeignKeyName => "WIRId";
    }
}
