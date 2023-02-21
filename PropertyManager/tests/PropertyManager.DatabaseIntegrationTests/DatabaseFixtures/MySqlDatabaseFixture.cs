namespace PropertyManager.Shared.DatabaseIntegrationTests.DatabaseFixtures
{
    using Microsoft.Extensions.Configuration;
    using MySql.Data.MySqlClient;
    using System;
    using System.Text;

    public class MySqlDatabaseFixture : IDisposable
    {

        public MySqlDatabaseFixture()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var settings = config.Build();

            ConnectionString = settings.GetConnectionString("MySql");
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            Dispose();
            connection.Open();
            PopulatePropertyTypes(connection);
            PopulateProperties(connection);
            connection.Close();
        }

        private void PopulatePropertyTypes(MySqlConnection connection)
        {
            var sql = new StringBuilder();
            sql.AppendLine("DELETE FROM propertytype;");
            sql.AppendLine("INSERT INTO propertytype (id, name) VALUES (1, 'Apartment');");
            sql.AppendLine("INSERT INTO propertytype (id, name) VALUES (2, 'Detached House');");
            sql.AppendLine("INSERT INTO propertytype (id, name) VALUES (3, 'Semi Detached House');");
            sql.AppendLine("INSERT INTO propertytype (id, name) VALUES (4, 'Terraced House');");
            sql.AppendLine("INSERT INTO propertytype (id, name) VALUES (5, 'Studio Flat');");

            using (MySqlCommand cmd = new MySqlCommand(sql.ToString(), connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void PopulateProperties(MySqlConnection connection)
        {
            var sql = new StringBuilder();
            sql.AppendLine("INSERT INTO propertyoverview (ID, type, purchase_price, purchase_date, garage, parking_spaces, notes) VALUES (1,1,100000, '2023/02/06',1,2,'some notes');");
            sql.AppendLine("INSERT INTO propertyaddress (id, line1, line2, city, region, postcode) VALUES (1, 'line 1', 'line 2', 'city', 'region', 'postcode');");
            sql.AppendLine("INSERT INTO propertyoverview (ID, type, purchase_price, purchase_date, garage, parking_spaces, notes) VALUES (2,1,100000, '2023/02/06',1,2,'some notes');");
            sql.AppendLine("INSERT INTO propertyaddress (id, line1, line2, city, region, postcode) VALUES (2, 'line 1', 'line 2', 'city', 'region', 'postcode');");
            using (MySqlCommand cmd = new MySqlCommand(sql.ToString(), connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void DeletePropertyTypes(MySqlConnection connection)
        {
            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM propertytype", connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteProperties(MySqlConnection connection)
        {
            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM propertyoverview", connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();
            DeleteProperties(connection);
            DeletePropertyTypes(connection);
            connection.Close();
        }

        public string ConnectionString { get; }
    }
}
