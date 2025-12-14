using Dubox.Domain.Services.ImageEntityConfig;

namespace Dubox.Infrastructure.Services.ImageEntityConfig.ImageEntityConfigFactory
{
    public class ProgressUpdateImageConfig : IImageEntityConfig
    {
        public string ForeignKeyName => "ProgressUpdateId";
    }

    public class QualityIssueImageConfig : IImageEntityConfig
    {
        public string ForeignKeyName => "QualityIssueId";
    }
    public class WIRCheckpointImageConfig : IImageEntityConfig
    {
        public string ForeignKeyName => "WIRId";
    }
}
