namespace DataAccess.database
{
    using Dapper;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Logging;

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
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async ValueTask<int> ExecuteCommand(string sql, IList<SqlParameter> parameters)
        {
            
            var connection = new SqlConnection(_connectionString);
            
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(sql, connection);

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            int affectedRecords = await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            return affectedRecords;
        }

        /// <inheritdoc />
        public async Task<dynamic?> GetSingleRowAsync(string sql, int id, string idColumnName = "id")
        {
            var parameters = new DynamicParameters();
            parameters.Add(idColumnName, id);
            using (var connection = new SqlConnection(_connectionString))
            {

                return await connection.QueryFirstOrDefaultAsync(sql, parameters);
            }            
        }
    }
}
