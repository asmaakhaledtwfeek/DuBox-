namespace Dubox.Domain.Services.ImageEntityConfig
{
    public interface IImageEntityConfig
    {
        string ForeignKeyName { get; }
    }

    public interface IImageEntityConfigFactory
    {
        IImageEntityConfig GetConfig<TEntity>();
    }

}
