namespace Service.property.DataStores
{
    using DataAccess.database;
    using System.Text;

    /// <summary>
    /// A property datastore that uses MySql as the backend
    /// </summary>
    public class MySqlPropertyDataStore : SqlPropertyDataStore
    {
        private string _InsertSql = string.Empty;

        /// <summary>
        /// Initialises a new instance of the <see cref="MySqlPropertyDataStore"/> class.
        /// </summary>
        /// <param name="connection">An instance of <see cref="IDatabaseConnection"/></param>
        /// <param name="logger">An instance of <see cref="ILogger"/></param>
        public MySqlPropertyDataStore(IDatabaseConnection connection, ILogger<MySqlPropertyDataStore> logger) : base(connection, logger) { }

        /// <inheritdoc />
        protected override string GetSql => "SELECT purchase_price, propertytype.id, propertytype.name, purchase_date, garage, parking_spaces, notes, propertyaddress.line1, propertyaddress.line2, propertyaddress.city, propertyaddress.region, propertyaddress.postcode FROM propertyoverview INNER JOIN propertytype ON propertytype.id = propertyoverview.type LEFT JOIN propertyaddress ON propertyaddress.id = propertyoverview.id WHERE propertyoverview.id = @id";

        /// <inheritdoc />
        protected override string InsertSql
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_InsertSql))
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("INSERT INTO propertyoverview (type, purchase_price, purchase_date, garage, parking_spaces, notes) VALUES (@type, @purchase_price, @purchase_date, @garage, @parking_spaces, @notes); SET @id = LAST_INSERT_ID();");
                    sql.Append("INSERT INTO propertyaddress (id, line1, line2, city, region, postcode) VALUES (@id, @line1, @line2, @city, @region, @postcode);");
                    sql.Append("SELECT cast(@id as unsigned)");
                    _InsertSql = sql.ToString();
                }

                return _InsertSql;
            }
        }

        /// <inheritdoc />
        protected override string UpdateSql => "UPDATE propertyoverview SET type = @type, purchase_price = @purchase_price, purchase_date = @purchase_date, garage = @garage, parking_spaces = @parking_spaces, notes = @notes WHERE id = @id;UPDATE propertyaddress SET line1 = @line1, line2 = @line2, city = @city, region = @region, postcode = @postcode where id = @id;SELECT CAST(@id as UNSIGNED)";

        /// <inheritdoc />
        protected override string DeleteSql => "DELETE FROM propertyoverview WHERE id = @id";
    }
}
