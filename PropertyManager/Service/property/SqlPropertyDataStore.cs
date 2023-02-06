namespace Service.property
{
    using DataAccess.database;
    using Microsoft.Data.SqlClient;
    using System.Text;

    /// <summary>
    /// A property datastore that uses a relational database as the backend
    /// </summary>
    public class SqlPropertyDataStore : IPropertyDataStore
    {
        private IDatabaseConnection _databaseConnection;

        /// <summary>
        /// Initialises a new intance of the <see cref="SqlPropertyDataStore"/> class.
        /// </summary>
        /// <param name="databaseConnection">An instance of <see cref="IDatabaseConnection"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SqlPropertyDataStore(IDatabaseConnection databaseConnection) 
        {
            _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
        }

        /// <inheritdoc />
        public async ValueTask<bool> DeleteAsync(int id)
        {

            string sql = "DELETE FROM property.overview WHERE id = @id";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@id", id)
            };
            int affectedRecords = await _databaseConnection.ExecuteCommand(sql, parameters);

            if (affectedRecords > 0)
                return true;

            return false;
        }

        /// <inheritdoc />
        public async ValueTask<IProperty?> GetAsync(int id)
        {
            var sql = "SELECT purchase_price, purchase_date, garage, parking_spaces, notes FROM propery.overview WHERE id = @id";
            var result = await _databaseConnection.GetSingleRowAsync(sql, id);

            if (result == null)
            {
                return null;
            }

            return new Property(id, PropertyType.Apartment, null, result.purchase_price, result.purchase_date, result.garage, result.parking_spaces, result.notes);
        }

        public async ValueTask<IProperty?> SaveAsync(IProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            StringBuilder sql = new StringBuilder();
            var parameters = new List<SqlParameter>();
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

            if (property.Id == 0)
            {
                parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.InputOutput});
                sql.Append("INSERT INTO property.overview (purchase_price, purchase_date, garage, parking_spaces, notes) VALUES (@purchase_price, @purchase_date, @garage, @parking_spaces, @notes); SET @id = scope_identity()"); ;
            }
            else
            {
                sql.Append("UPDATE property.overview SET puchase_price = @purchase_price, purchase_date = @purchase_date, garage = @garage, parking_spaces = @parking_spaces, notes = @notes WHERE id = @id");
            }

            await _databaseConnection.ExecuteCommand(sql.ToString(), parameters);
            return property;
        }
    }
}
