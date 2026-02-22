using System.Data.Common;

namespace Dubox.Application.Abstractions;

/// <summary>
/// Provides generic, provider-agnostic ADO.NET query execution.
/// Use this instead of writing raw connection/command/reader boilerplate
/// in every handler that needs a fast SQL query.
///
/// Parameters are expressed as (name, value) tuples so the Application
/// layer never references provider-specific types (SqlParameter, etc.).
/// The implementation in Infrastructure creates the correct DbParameter
/// instances via DbCommand.CreateParameter().
/// </summary>
public interface IDbQueryExecutor
{
    /// <summary>
    /// Executes <paramref name="sql"/> and maps the first row to <typeparamref name="T"/>.
    /// Returns <c>default(T)</c> when the result set is empty.
    /// </summary>
    Task<T?> QuerySingleAsync<T>(
        string sql,
        Func<DbDataReader, T> map,
        IEnumerable<(string Name, object? Value)>? parameters = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes <paramref name="sql"/> and maps every row to <typeparamref name="T"/>.
    /// Returns an empty list when the result set is empty.
    /// </summary>
    Task<List<T>> QueryListAsync<T>(
        string sql,
        Func<DbDataReader, T> map,
        IEnumerable<(string Name, object? Value)>? parameters = null,
        CancellationToken cancellationToken = default);
}
