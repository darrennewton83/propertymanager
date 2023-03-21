namespace PropertyManager.Shared.PropertyType.DataStores
{
    using DataAccess.Database;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A property type datastore that uses a relational database as the backend
    /// </summary>
    public class MySqlPropertyTypeDataStore : SqlPropertyTypeDataStore
    {
        /// <summary>
        /// Initialises a new intance of the <see cref="SqlPropertyTypeDataStore"/> class.
        /// </summary>
        /// <param name="databaseConnection">An instance of <see cref="IDatabaseConnection"/></param>
        /// <param name="loggern">An instance of <see cref="ILogger"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MySqlPropertyTypeDataStore(IDatabaseConnection databaseConnection, ILogger<SqlPropertyTypeDataStore> logger) : base(databaseConnection, logger)
        {
        }

        /// <inheritdoc />
        protected override string GetSql => "SELECT id, name FROM propertytype WHERE id = @id";

        /// <inheritdoc />
        protected override string GetByNameSql => "SELECT id, name FROM property.type WHERE name = @name";

        /// <inheritdoc />
        protected override string InsertSql => "INSERT INTO propertytype (name) VALUES (@name);select LAST_INSERT_ID();";

        /// <inheritdoc />
        protected override string UpdateSql => "UPDATE propertytype SET name = @name where id = @id";

        /// <inheritdoc />
        protected override string DeleteSql => "DELETE FROM propertytype WHERE id = @id";

        protected override string GetAllSql => "SELECT id, name FROM propertytype ORDER BY name";
    }
}
