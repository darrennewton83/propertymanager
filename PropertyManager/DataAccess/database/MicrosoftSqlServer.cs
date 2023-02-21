namespace DataAccess.database
{
    using Dapper;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Logging;
    using System.Data;
    using System.Data.Common;

    /// <summary>
    /// An database connection for Microsoft SQL Server
    /// </summary>
    public class MicrosoftSqlServer : IDatabaseConnection, IDisposable
    {
        ILogger<MicrosoftSqlServer> _logger;
        private string _connectionString;

        /// <param name="logger">The logger</param>
        /// <param name="connectionString">The connection string of the database to connect to</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MicrosoftSqlServer(ILogger<MicrosoftSqlServer> logger, string connectionString)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            _connectionString = connectionString;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async ValueTask<int> ExecuteCommandAsync(string sql, IList<SqlParameter> parameters)
        {
            
            var connection = new SqlConnection(_connectionString);
            
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(sql, connection);

            if (parameters != null && parameters.Count > 0)
            {
                InitialiseParameters(command, parameters);
            }
            
            int affectedRecords = await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            return affectedRecords;
        }

        /// <inheritdoc />
        public async ValueTask<object?> ExecuteScalarAsync(string sql)
        {
            return await ExecuteScalarAsync(sql, null);
        }

        /// <inheritdoc />
        public async ValueTask<object?> ExecuteScalarAsync(string sql, IList<SqlParameter> parameters)
        {
            var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(sql, connection);

            if (parameters != null && parameters.Count > 0)
            {
                InitialiseParameters(command, parameters);
            }

            var result = await command.ExecuteScalarAsync();
            await connection.CloseAsync();
            return result;
        }        

        /// <inheritdoc />
        public async Task<object> GetSingleRowAsync(string sql, int id, Func<dynamic, object> InitialiseReturnClass, string idColumnName = "id")
        {
            var parameters = new DynamicParameters();
            parameters.Add(idColumnName, id);
            using (var connection = new SqlConnection(_connectionString))
            {

                var result = await connection.QueryFirstOrDefaultAsync(sql, parameters);
                if (result == null)
                    return null;

                return InitialiseReturnClass(result); ;
            }            
        }

        /// <inheritdoc />
        public async Task<object> GetSingleRowAsync(string sql, string id, Func<dynamic, object> InitialiseReturnClass, string idColumnName = "id")
        {
            var parameters = new DynamicParameters();
            parameters.Add(idColumnName, id);
            using (var connection = new SqlConnection(_connectionString))
            {

                var result = await connection.QueryFirstOrDefaultAsync(sql, parameters);
                if (result == null)
                    return null;

                return InitialiseReturnClass(result); ;
            }
        }

        /// <inheritdoc />
        public void InitialiseParameters(IDbCommand command, IList<SqlParameter> parameters)
        {
            if (command is SqlCommand sqlCommand) 
            {
                sqlCommand.Parameters.AddRange(parameters.ToArray());
            }
        }
    }
}
