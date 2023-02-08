﻿namespace Service.propertyType.DataStores
{
    using DataAccess.database;
    using Microsoft.Data.SqlClient;

    public abstract class SqlPropertyTypeDataStore : IPropertyTypeDataStore
    {
        protected IDatabaseConnection _databaseConnection;
        protected ILogger<SqlPropertyTypeDataStore> _logger;

        public SqlPropertyTypeDataStore(IDatabaseConnection databaseConnection, ILogger<SqlPropertyTypeDataStore> logger)
        {
            _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
       
        /// <inheritdoc />
        public async ValueTask<IPropertyType?> GetAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var result = await _databaseConnection.GetSingleRowAsync(GetSql, id, (row) =>
            {
                return new PropertyType(id, row.name);
            });

            if (result is PropertyType propertyType)
            {
                return propertyType;
            }

            return null;
        }

        /// <inheritdoc />
        public async ValueTask<IPropertyType?> SaveAsync(IPropertyType propertyType)
        {
            if (propertyType == null)
            {
                throw new ArgumentNullException(nameof(propertyType));
            }

            string sql;
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@name", propertyType.Name)
            };

            if (propertyType.Id.HasValue) //update
            {
                parameters.Add(new SqlParameter("@id", propertyType.Id.Value));
                sql = UpdateSql;
            }
            else
            {
                sql = InsertSql;
            }

            object? result = await _databaseConnection.ExecuteScalarAsync(sql.ToString(), parameters);

            if (result == null)
            {
                return null;
            }

            if (!propertyType.Id.HasValue)
            {
                int id = Convert.ToInt32(result);

                if (id <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(id));
                }

                propertyType = new PropertyType(id, propertyType.Name);
            }

            return propertyType;
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

        /// <summary>
        /// Gets the SQL required to retreive a record
        /// </summary>
        protected abstract string GetSql { get; }

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
