namespace PropertyManager.Shared.DatabaseIntegrationTests.DatabaseFixtures
{
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Text;

    public class SqlServerDatabaseFixture : IDisposable
    {

        public SqlServerDatabaseFixture()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var settings = config.Build();

            ConnectionString = settings.GetConnectionString("SqlServer");
            SqlConnection connection = new SqlConnection(ConnectionString);
            Dispose();
            connection.Open();

            PopulatePropertyTypes(connection);
            PopulateProperties(connection);
            connection.Close();
        }

        private void PopulatePropertyTypes(SqlConnection connection)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SET IDENTITY_INSERT property.type ON");
            sql.AppendLine("INSERT INTO property.type (id, name) VALUES (1, 'Apartment')");
            sql.AppendLine("INSERT INTO property.type (id, name) VALUES (2, 'Detached House')");
            sql.AppendLine("INSERT INTO property.type (id, name) VALUES (3, 'Semi Detached House')");
            sql.AppendLine("INSERT INTO property.type (id, name) VALUES (4, 'Terraced House')");
            sql.AppendLine("INSERT INTO property.type (id, name) VALUES (5, 'Studio Flat')");
            sql.AppendLine("SET IDENTITY_INSERT property.type OFF");

            using (SqlCommand cmd = new SqlCommand(sql.ToString(), connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void PopulateProperties(SqlConnection connection)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SET IDENTITY_INSERT property.overview ON");
            sql.AppendLine("INSERT INTO property.overview (ID, type, purchase_price, purchase_date, garage, parking_spaces, notes) VALUES (1,1,100000, '2023/02/06',1,2,'some notes');");
            sql.AppendLine("INSERT INTO property.address (id, line1, line2, city, region, postcode) VALUES (1, 'line 1', 'line 2', 'city', 'region', 'postcode');");
            sql.AppendLine("INSERT INTO property.overview (ID, type, purchase_price, purchase_date, garage, parking_spaces, notes) VALUES (2,1,100000, '2023/02/06',1,2,'some notes');");
            sql.AppendLine("INSERT INTO property.address (id, line1, line2, city, region, postcode) VALUES (2, 'line 1', 'line 2', 'city', 'region', 'postcode');");
            sql.AppendLine("SET IDENTITY_INSERT property.overview OFF");

            using (SqlCommand cmd = new SqlCommand(sql.ToString(), connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void DeletePropertyTypes(SqlConnection connection)
        {
            using (SqlCommand cmd = new SqlCommand("DELETE FROM property.type", connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteProperties(SqlConnection connection)
        {
            using (SqlCommand cmd = new SqlCommand("DELETE FROM property.overview", connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            DeleteProperties(connection);
            DeletePropertyTypes(connection);
            connection.Close();
        }

        public string ConnectionString { get; }
    }
}
