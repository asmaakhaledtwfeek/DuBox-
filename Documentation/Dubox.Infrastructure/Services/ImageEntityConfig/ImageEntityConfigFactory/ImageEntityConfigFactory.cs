using Dubox.Domain.Entities;
using Dubox.Infrastructure.Services.ImageEntityConfig.ImageEntityConfigFactory;
namespace Dubox.Domain.Services.ImageEntityConfig.ImageEntityConfigFactory
{
    public class ImageEntityConfigFactory : IImageEntityConfigFactory
    {
        private readonly Dictionary<Type, IImageEntityConfig> _configs =
            new()
            {
            { typeof(ProgressUpdateImage), new ProgressUpdateImageConfig() },
            { typeof(QualityIssueImage), new QualityIssueImageConfig() },
             { typeof(WIRCheckpointImage), new WIRCheckpointImageConfig() }
            };

        public IImageEntityConfig GetConfig<TEntity>()
        {
            var type = typeof(TEntity);

            if (_configs.TryGetValue(type, out var config))
                return config;

            throw new InvalidOperationException($"No config registered for entity {type.Name}");
        }
    }

}
