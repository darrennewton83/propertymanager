namespace PropertyManager.Shared.Property.DataStores
{
    using DataAccess.Database;
    using Microsoft.Extensions.Logging;
    using System.Text;

    /// <summary>
    /// A property datastore that uses Microsoft SQL Server as the backend
    /// </summary>
    public class MicrosoftSqlServerPropertyDataStore : SqlPropertyDataStore
    {
        private string _InsertSql = string.Empty;

        /// <summary>
        /// Initialises a new instance of the <see cref="MicrosoftSqlServerPropertyDataStore"/> class.
        /// </summary>
        /// <param name="connection">An instance of <see cref="IDatabaseConnection"/></param>
        /// <param name="logger">An instance of <see cref="ILogger"/></param>
        public MicrosoftSqlServerPropertyDataStore(IDatabaseConnection connection, ILogger<MicrosoftSqlServerPropertyDataStore> logger) : base(connection, logger) 
        { 
        }

        /// <inheritdoc />
        protected override string GetSql => "SELECT purchase_price, type.id, type.name, purchase_date, garage, parking_spaces, notes, property.address.line1, property.address.line2, property.address.city, property.address.region, property.address.postcode FROM property.overview INNER JOIN property.type ON property.type.id = property.overview.type LEFT JOIN property.address ON property.address.id = property.overview.id WHERE property.overview.id = @id";

        /// <inheritdoc />
        protected override string InsertSql
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_InsertSql))
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("INSERT INTO property.overview (type, purchase_price, purchase_date, garage, parking_spaces, notes) VALUES (@type, @purchase_price, @purchase_date, @garage, @parking_spaces, @notes); SET @id = scope_identity();");
                    sql.Append("INSERT INTO property.address (id, line1, line2, city, region, postcode) VALUES (@id, @line1, @line2, @city, @region, @postcode);");
                    sql.Append("SELECT @id");
                    _InsertSql = sql.ToString();
                }

                return _InsertSql;
            }
        }

        /// <inheritdoc />
        protected override string UpdateSql => "UPDATE property.overview SET type = @type, purchase_price = @purchase_price, purchase_date = @purchase_date, garage = @garage, parking_spaces = @parking_spaces, notes = @notes WHERE id = @id;UPDATE property.address SET line1 = @line1, line2 = @line2, city = @city, region = @region, postcode = @postcode where id = @id;SELECT @id";

        /// <inheritdoc />
        protected override string DeleteSql => "DELETE FROM property.overview WHERE id = @id";
    }
}
