using Dubox.Application.Abstractions;
using Dubox.Domain.Abstraction;
using System.Data;
using System.Data.Common;

namespace Dubox.Infrastructure.Services;

/// <summary>
/// ADO.NET query executor.
/// Retrieves the underlying DbConnection from EF's IDbContext so all
/// queries share the same connection (and transaction, if one is active).
/// </summary>
internal sealed class DbQueryExecutor : IDbQueryExecutor
{
    private readonly IDbContext _context;

    public DbQueryExecutor(IDbContext context)
    {
        _context = context;
    }

    // -------------------------------------------------------------------------
    // Public API
    // -------------------------------------------------------------------------

    public async Task<T?> QuerySingleAsync<T>(
        string sql,
        Func<DbDataReader, T> map,
        IEnumerable<(string Name, object? Value)>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await using var cmd = await CreateCommandAsync(sql, parameters, cancellationToken);
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

        return await reader.ReadAsync(cancellationToken)
            ? map(reader)
            : default;
    }

    public async Task<List<T>> QueryListAsync<T>(
        string sql,
        Func<DbDataReader, T> map,
        IEnumerable<(string Name, object? Value)>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await using var cmd = await CreateCommandAsync(sql, parameters, cancellationToken);
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

        var results = new List<T>();
        while (await reader.ReadAsync(cancellationToken))
            results.Add(map(reader));

        return results;
    }

    // -------------------------------------------------------------------------
    // Private helpers
    // -------------------------------------------------------------------------

    /// <summary>
    /// Opens the connection (if needed) and returns a fully configured DbCommand.
    /// The command is scoped to the caller via <c>await using</c>.
    /// The connection itself is NOT disposed here — EF owns its lifetime.
    /// </summary>
    private async Task<DbCommand> CreateCommandAsync(
        string sql,
        IEnumerable<(string Name, object? Value)>? parameters,
        CancellationToken cancellationToken)
    {
        var connection = _context.GetDbConnection();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);

        var cmd = connection.CreateCommand();
        cmd.CommandText = sql;

        if (parameters is not null)
        {
            foreach (var (name, value) in parameters)
            {
                var p = cmd.CreateParameter();
                p.ParameterName = name;
                p.Value = value ?? DBNull.Value;
                cmd.Parameters.Add(p);
            }
        }

        return cmd;
    }
}
