namespace DatabaseIntegrationTests.property
{
    using DatabaseIntegrationTests.DatabaseFixtures;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using Service.property;
    using Service.property.DataStores;
    using Service.propertyType;

    public class MySqlDataStoreTests
    {
        public class SaveAsync : Fixture
        {
            [Fact]
            public async void GivenNewPropertyToSave_SaveAsync_SavesProperty()
            {
                var sut = new MySqlPropertyDataStore(new DataAccess.database.MySql(this.DatabaseLogger, ConnectionString), Logger);

                var property = new Property(new PropertyType(1, "Apartment"), new Service.address.Address("line 1", "line 2", "City", "County", "postcode"), 100000, new DateOnly(2023, 02, 06), true, 2, "some notes");
                var result = await sut.SaveAsync(property);

                var savedProperty = Assert.IsType<Property>(result);
                Assert.True(savedProperty.Id.HasValue);
            }

            [Fact]
            public async void GivenExistingPropertyToSave_SaveAsync_SavesProperty()
            {
                var sut = new MySqlPropertyDataStore(new DataAccess.database.MySql(this.DatabaseLogger, ConnectionString), Logger);

                var property = new Property(1, new PropertyType(2, "Apartment"), new Service.address.Address("line 1 updated", "line 2 updated", "City updated", "County updated", "postcode updated"), 50000, new DateOnly(2022, 02, 06), false, 3, "some notes updated");
                var result = await sut.SaveAsync(property);

                Assert.IsType<Property>(result);
                var savedProperty = await sut.GetAsync(1);

                Assert.Equal(2, savedProperty.Type.Id.Value);
                Assert.Equal("line 1 updated", savedProperty.Address.AddressLine1);
                Assert.Equal("line 2 updated", savedProperty.Address.AddressLine2);
                Assert.Equal("City updated", savedProperty.Address.City);
                Assert.Equal("County updated", savedProperty.Address.Region);
                Assert.Equal("postcode updated", savedProperty.Address.PostalCode);
                Assert.Equal(50000, savedProperty.PurchasePrice);
                Assert.Equal(new DateOnly(2022, 02, 06), savedProperty.PurchaseDate);
                Assert.False(savedProperty.Garage);
                Assert.Equal((byte)3, savedProperty.NumberOfParkingSpaces);
                Assert.Equal("some notes updated", savedProperty.Notes);
            }
        }

        public class GetAsync : Fixture
        {
            [Fact]
            public async void GivenPropertyExists_GetAsync_ReturnsProperty()
            {
                var sut = new MySqlPropertyDataStore(new DataAccess.database.MySql(this.DatabaseLogger, ConnectionString), Logger);
                var result = await sut.GetAsync(1);
                Assert.IsType<Property>(result);
            }

            [Fact]
            public async void GivenPropertyDoesNotExist_GetAsync_ReturnsNull()
            {
                var sut = new MySqlPropertyDataStore(new DataAccess.database.MySql(this.DatabaseLogger, ConnectionString), Logger);
                var result = await sut.GetAsync(999);
                Assert.Null(result);
            }
        }

        public class DeleteAsync : Fixture
        {
            [Fact]
            public async void GivenRecordExists_DeleteAsync_ReturnsTrue()
            {
                var sut = new MySqlPropertyDataStore(new DataAccess.database.MySql(this.DatabaseLogger, ConnectionString), Logger);
                var result = await sut.DeleteAsync(2);
                Assert.True(result);

                var connection = new DataAccess.database.MySql(this.DatabaseLogger, ConnectionString);
                var count = await connection.ExecuteScalarAsync("select COUNT(id) from propertyoverview where id = 2");
                Assert.Equal((long)0, count);
            }
        }

        public class Fixture : MySqlDatabaseFixture
        {
            public ILogger<DataAccess.database.MySql> DatabaseLogger;
            public ILogger<MySqlPropertyDataStore> Logger;

            public Fixture()
            {
                DatabaseLogger = Substitute.For<ILogger<DataAccess.database.MySql>>();
                Logger = Substitute.For<ILogger<MySqlPropertyDataStore>>();
            }
        }
    }
}
