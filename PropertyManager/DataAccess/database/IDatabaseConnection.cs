namespace DataAccess.database
{
    using Microsoft.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// Represents a connection to a database to execute commands against
    /// </summary>
    public interface IDatabaseConnection
    {
        /// <summary>
        /// Executes a command against a database
        /// </summary>
        /// <param name="sql">The SQL query to run</param>
        /// <param name="parameters">A collection of parameters</param>
        /// <returns>An integer representing the number of rows affected</returns>
        ValueTask<int> ExecuteCommandAsync(string sql, IList<SqlParameter> parameters);

        /// <summary>
        /// Executes a command against a database and returns the result of the first column
        /// </summary>
        /// <param name="sql">The SQL query to run</param>
        /// <returns>An integer representing the number of rows affected</returns>
        ValueTask<object?> ExecuteScalarAsync(string sql);

        /// <summary>
        /// Executes a command against a database and returns the result of the first column
        /// </summary>
        /// <param name="sql">The SQL query to run</param>
        /// <param name="parameters">A collection of parameters</param>
        /// <returns>An integer representing the number of rows affected</returns>
        ValueTask<object?> ExecuteScalarAsync(string sql, IList<SqlParameter> parameters);

        /// <summary>
        /// Returns a single row from a database
        /// </summary>
        /// <param name="sql">The SQL query to run</param>
        /// <param name="id">The unique identifer of the row to return</param>
        /// <param name="idColumnName">The name of the id field in the database. Default is "id"</param>
        /// <returns>The row and its fields or null if row cannot be found</returns>
        Task<object> GetSingleRowAsync(string sql, int id, Func<dynamic, object> InitialiseReturnClass, string idColumnName = "id");

        /// <summary>
        /// Returns a single row from a database
        /// </summary>
        /// <param name="sql">The SQL query to run</param>
        /// <param name="id">The unique identifer of the row to return</param>
        /// <param name="idColumnName">The name of the id field in the database. Default is "id"</param>
        /// <returns>The row and its fields or null if row cannot be found</returns>
        Task<object> GetSingleRowAsync(string sql, string id, Func<dynamic, object> InitialiseReturnClass, string idColumnName = "id");

        /// <summary>
        /// Initialises the parameters variable to pass to a command
        /// </summary>
        /// <param name="command">An instance of <see cref="IDbCommand"/></param>
        /// <param name="parameters">The parameters to pass to the command</param>
        abstract void InitialiseParameters(IDbCommand command, IList<SqlParameter> parameters);
    }
}
