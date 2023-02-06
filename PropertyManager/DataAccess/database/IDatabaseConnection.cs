namespace DataAccess.database
{
    using Microsoft.Data.SqlClient;

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
        ValueTask<int> ExecuteCommand(string sql, IList<SqlParameter> parameters);

        /// <summary>
        /// Returns a single row from a database
        /// </summary>
        /// <param name="sql">The SQL query to run</param>
        /// <param name="id">The unique identifer of the row to return</param>
        /// <param name="idColumnName">The name of the id field in the database. Default is "id"</param>
        /// <returns>The row and its fields or null if row cannot be found</returns>
        Task<dynamic?> GetSingleRowAsync(string sql, int id, string idColumnName = "id");
    }
}
