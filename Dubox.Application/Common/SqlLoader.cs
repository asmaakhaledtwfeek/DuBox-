using System.Reflection;

namespace Dubox.Application.Common;

/// <summary>
/// Loads embedded .sql resource files at runtime.
///
/// Convention: the .sql file must live in the same folder as the calling
/// type and share the same base name as the class, or be passed explicitly.
///
/// Examples
/// --------
/// // Auto-detect: looks for MyHandler.sql next to MyHandler.cs
/// string sql = SqlLoader.LoadFor<MyHandler>();
///
/// // Explicit: load by assembly-qualified resource path
/// string sql = SqlLoader.Load("Dubox.Application.Features.X.Queries.Sql.MyQuery.sql");
/// </summary>
public static class SqlLoader
{
    private static readonly Assembly _assembly = typeof(SqlLoader).Assembly;

    /// <summary>
    /// Loads the .sql file whose embedded resource name matches
    /// <c>&lt;namespace of T&gt;.Sql.&lt;name of T without "QueryHandler"&gt;.sql</c>.
    ///
    /// E.g. <c>GetActivityReviewsSummaryQueryHandler</c> maps to
    /// <c>…Queries.Sql.GetActivityReviewsSummary.sql</c>.
    /// </summary>
    public static string LoadFor<T>()
    {
        var type     = typeof(T);
        var typeName = type.Name.Replace("QueryHandler", "").Replace("CommandHandler", "");
        var ns       = type.Namespace ?? string.Empty;
        var resource = $"{ns}.Sql.{typeName}.sql";

        return Load(resource);
    }

    /// <summary>
    /// Loads an embedded .sql resource by its full assembly-qualified name.
    /// </summary>
    public static string Load(string resourceName)
    {
        using var stream = _assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException(
                $"Embedded SQL resource '{resourceName}' not found. " +
                $"Make sure the file is marked as EmbeddedResource in the .csproj.\n" +
                $"Available resources:\n  " +
                string.Join("\n  ", _assembly.GetManifestResourceNames()));

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
