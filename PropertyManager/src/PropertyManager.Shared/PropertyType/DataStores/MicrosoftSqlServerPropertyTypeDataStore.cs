namespace PropertyManager.Shared.PropertyType.DataStores
{
    using DataAccess.Database;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A property type datastore that uses a relational database as the backend
    /// </summary>
    public class MicrosoftSqlServerPropertyTypeDataStore : SqlPropertyTypeDataStore
    {
        /// <summary>
        /// Initialises a new intance of the <see cref="MicrosoftSqlServerPropertyTypeDataStore"/> class.
        /// </summary>
        /// <param name="databaseConnection">An instance of <see cref="IDatabaseConnection"/></param>
        /// <param name="loggern">An instance of <see cref="ILogger"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MicrosoftSqlServerPropertyTypeDataStore(IDatabaseConnection databaseConnection, ILogger<MicrosoftSqlServerPropertyTypeDataStore> logger) : base(databaseConnection, logger)
        {
        }

        /// <inheritdoc />
        protected override string GetSql => "SELECT id, name FROM property.type WHERE id = @id";

        /// <inheritdoc />
        protected override string GetByNameSql => "SELECT id, name FROM property.type WHERE name = @name";

        /// <inheritdoc />
        protected override string InsertSql => "INSERT INTO property.type (name) VALUES (@name);SELECT scope_identity()";

        /// <inheritdoc />
        protected override string UpdateSql => "UPDATE property.type SET name = @name where id = @id";

        /// <inheritdoc />
        protected override string DeleteSql => "DELETE FROM property.type WHERE id = @id";      
    }
}
