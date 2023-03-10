namespace PropertyManager.Shared.Property.DataStores
{
    using DataAccess.Database;
    using Microsoft.Data.SqlClient;
    using PropertyManager.Shared.Address;
    using PropertyManager.Shared.EntityResults;
    using PropertyManager.Shared.Property;
    using PropertyManager.Shared.PropertyType;
    using Microsoft.Extensions.Logging;
    using Mysqlx.Resultset;

    /// <summary>
    /// A property datastore that uses a relational database as the backend
    /// </summary>
    public abstract class SqlPropertyDataStore : IPropertyDataStore
    {
        private IDatabaseConnection _databaseConnection;
        private ILogger<SqlPropertyDataStore> _logger;

        /// <summary>
        /// Initialises a new intance of the <see cref="SqlPropertyDataStore"/> class.
        /// </summary>
        /// <param name="databaseConnection">An instance of <see cref="IDatabaseConnection"/></param>
        /// <param name="logger">An instance of <see cref="ILogger"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SqlPropertyDataStore(IDatabaseConnection databaseConnection, ILogger<SqlPropertyDataStore> logger)
        {
            _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async ValueTask<bool> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@id", id)
            };
            int affectedRecords = await _databaseConnection.ExecuteCommandAsync(DeleteSql, parameters);

            if (affectedRecords > 0)
                return true;

            return false;
        }

        
        public async ValueTask<IProperty?> GetAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var result = await _databaseConnection.GetSingleRowAsync(GetSql, id, (row) =>
            {
                DateOnly? purchasePrice = null;
                var propertyType = new PropertyType(row.id, row.name);
                var address = new Address(row.line1, row.line2, row.city, row.region, row.postcode);

                DateOnly? purchaseDate = null;
                if (row.purchase_date != null)
                {
                    purchaseDate = new DateOnly(row.purchase_date.Year, row.purchase_date.Month, row.purchase_date.Day);
                }

                return new Property(id, propertyType, address, row.purchase_price, purchaseDate, row.garage, row.parking_spaces, row.notes);
            });

            if (result is Property property)
            {
                return property;
            }

            return null;
        }

        /// <inheritdoc />
        public async ValueTask<IEnumerable<IProperty>> GetAsync()
        {
            var result = await _databaseConnection.QueryAsync(GetAllSql, (rows) =>
            {
                var properties = new List<Property>();
                foreach (var row in rows)
                {
                    DateOnly? purchasePrice = null;
                    var propertyType = new PropertyType(row.id, row.name);
                    var address = new Address(row.line1, row.line2, row.city, row.region, row.postcode);

                    DateOnly? purchaseDate = null;
                    if (row.purchase_date != null)
                    {
                        purchaseDate = new DateOnly(row.purchase_date.Year, row.purchase_date.Month, row.purchase_date.Day);
                    }

                    properties.Add(new Property(row.id, propertyType, address, row.purchase_price, purchaseDate, row.garage, row.parking_spaces, row.notes));
                }
                return properties;
            });

            if (result is IEnumerable<IProperty> properties)
            {
                return properties;
            }

            return Enumerable.Empty<IProperty>();
        }

        /// <inheritdoc />
        public async ValueTask<IEntityResult<IProperty>> SaveAsync(IProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            string sql;
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@type", property.Type.Id)
            };
            if (property.PurchasePrice.HasValue)
            {
                parameters.Add(new SqlParameter("@purchase_price", property.PurchasePrice));
            }
            else
            {
                parameters.Add(new SqlParameter("@purchase_price", DBNull.Value));
            }

            if (property.PurchaseDate.HasValue)
            {
                parameters.Add(new SqlParameter("@purchase_date", property.PurchaseDate));
            }
            else
            {
                parameters.Add(new SqlParameter("@purchase_date", DBNull.Value));
            }

            parameters.Add(new SqlParameter("@garage", property.Garage));
            if (property.NumberOfParkingSpaces.HasValue)
            {
                parameters.Add(new SqlParameter("@parking_spaces", property.NumberOfParkingSpaces));
            }
            else
            {
                parameters.Add(new SqlParameter("@parking_spaces", DBNull.Value));
            }

            if (string.IsNullOrWhiteSpace(property.Notes))
            {
                parameters.Add(new SqlParameter("@notes", DBNull.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("@notes", property.Notes));
            }

            parameters.Add(new SqlParameter("@line1", property.Address.AddressLine1) { Size = 250 });

            if (string.IsNullOrWhiteSpace(property.Address.AddressLine2))
            {
                parameters.Add(new SqlParameter("@line2", DBNull.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("@line2", property.Address.AddressLine2) { Size = 250 });
            }

            parameters.Add(new SqlParameter("@city", property.Address.City) { Size = 250 });
            if (string.IsNullOrWhiteSpace(property.Address.Region))
            {
                parameters.Add(new SqlParameter("@region", DBNull.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("@region", property.Address.Region) { Size = 250 });
            }

            parameters.Add(new SqlParameter("@postcode", property.Address.PostalCode) { Size = 50 });
            if (property.Id.HasValue)
            {
                parameters.Add(new SqlParameter("@id", property.Id.Value));
                sql = UpdateSql;
            }
            else
            {
                parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });
                sql = InsertSql;
            }

            object? result = null;

            try
            {
                result = await _databaseConnection.ExecuteScalarAsync(sql.ToString(), parameters);
            }
            catch (SqlException ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "SQL could not be run saving a Property Type.");
                }

                return new EntityErrorResult<IProperty>();
            }

            if (result == null)
            {
                return null;
            }

            if (!property.Id.HasValue)
            {
                int id = Convert.ToInt32(result);

                if (id <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(id));
                }
                property = new Property(id, property.Type, property.Address, property.PurchasePrice, property.PurchaseDate, property.Garage, property.NumberOfParkingSpaces, property.Notes);
            }

            return new ValueResult<IProperty>(property);
        }

        /// <summary>
        /// Gets the SQL required to retreive a record
        /// </summary>
        protected abstract string GetSql { get; }

        /// <summary>
        /// Gets the SQL required to retreive all records
        /// </summary>
        protected abstract string GetAllSql { get; }

        /// <summary>
        /// Gets the SQL required to insert a record
        /// </summary>
        protected abstract string InsertSql { get; }

        /// <summary>
        /// Gets the SQL required to update a record
        /// </summary>
        protected abstract string UpdateSql { get; }

        /// <summary>
        /// Gets the SQL required to delete a record
        /// </summary>
        protected abstract string DeleteSql { get; }
    }
}
