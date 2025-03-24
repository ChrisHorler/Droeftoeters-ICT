using Dapper;
using Microsoft.Data.SqlClient;

namespace droeftoeters_api.Interfaces;

public class SqlDataService : IDataService
{
    private readonly IConfiguration _config;
    private const string CONN = "azure";
    public SqlDataService(IConfiguration config)
    {
        _config = config;
    }
    
    public IEnumerable<T> QuerySql<T>(string query, object? parameters=null)
    {
        using var connection = CreateMySqlConnection();
        IEnumerable<T> result = connection.Query<T>(query, parameters);

        return result;
    }
    
    public T QueryFirstSql<T>(string query, object? parameters = null) => QuerySql<T>(query, parameters).First();
    
    public bool ExecuteSql(string query, object? parameters=null)
    {
        using var connection = CreateMySqlConnection();
        bool result = connection.Execute(query, parameters) > 0;

        return result;
    }
    
    private SqlConnection CreateMySqlConnection()
    {
        return new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING_SQL") ?? _config.GetConnectionString(CONN) ?? "");
    }
}