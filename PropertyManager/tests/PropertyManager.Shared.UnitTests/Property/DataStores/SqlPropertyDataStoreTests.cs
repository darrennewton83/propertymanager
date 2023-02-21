namespace PropertyManager.Shared.UnitTests.Property.DataStores
{
    using PropertyManager.DataAccess.Database;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using NSubstitute.ReturnsExtensions;
    using PropertyManager.Shared.Property;
    using PropertyManager.Shared.Property.DataStores;
    using PropertyManager.Shared.PropertyType;
    using PropertyManager.Shared.Address;

    public class SqlPropertyDataStoreTests
    {
        public class Ctor : Fixture
        {
            [Fact]
            public void GivenNullDatabaseConnection_ThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(() => new MicrosoftSqlServerPropertyDataStore(null, Logger));
            }

            [Fact]
            public void GivenNullLogger_ThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(() => new MicrosoftSqlServerPropertyDataStore(Connection, null));
            }
        }
        public class SaveAsync : Fixture
        {
            [Fact]
            public void GivenNullProperty_SaveAsync_ThrowsArgumentNullException()
            {
                var sut = new MicrosoftSqlServerPropertyDataStore(Connection, Logger);
                Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.SaveAsync(null));
            }

            [Fact]
            public async void GivenNewPropertyToSave_SaveAsync_SavesProperty()
            {
                var sut = new MicrosoftSqlServerPropertyDataStore(Connection, Logger);

                var property = new Property(new PropertyType(1, "Apartment"), new Address("line 1", "line 2", "City", "County", "postcode"), 100000, new DateOnly(2023, 02, 06), true, 2, "some notes");



                Connection.ExecuteScalarAsync("INSERT INTO property.overview (type, purchase_price, purchase_date, garage, parking_spaces, notes) VALUES (@type, @purchase_price, @purchase_date, @garage, @parking_spaces, @notes); SET @id = scope_identity();INSERT INTO property.address (id, line1, line2, city, region, postcode) VALUES (@id, @line1, @line2, @city, @region, @postcode);SELECT @id",
                    Arg.Is<List<SqlParameter>>(parameters =>
                parameters[0].ParameterName == "@type" && (int)parameters[0].Value == property.Type.Id &&
                parameters[1].ParameterName == "@purchase_price" && (decimal)parameters[1].Value == property.PurchasePrice &&
                parameters[2].ParameterName == "@purchase_date" && (DateOnly)parameters[2].Value == property.PurchaseDate &&
                parameters[3].ParameterName == "@garage" && (bool)parameters[3].Value == property.Garage &&
                parameters[4].ParameterName == "@parking_spaces" && (byte)parameters[4].Value == property.NumberOfParkingSpaces &&
                parameters[5].ParameterName == "@notes" && (string)parameters[5].Value == property.Notes &&
                parameters[6].ParameterName == "@line1" && (string)parameters[6].Value == property.Address.AddressLine1 &&
                parameters[7].ParameterName == "@line2" && (string)parameters[7].Value == property.Address.AddressLine2 &&
                parameters[8].ParameterName == "@city" && (string)parameters[8].Value == property.Address.City &&
                parameters[9].ParameterName == "@region" && (string)parameters[9].Value == property.Address.Region &&
                parameters[10].ParameterName == "@postcode" && (string)parameters[10].Value == property.Address.PostalCode
                )).Returns(2);
                var result = await sut.SaveAsync(property);

                var savedProperty = Assert.IsType<Property>(result.Value);
                Assert.Equal(2, savedProperty.Id.Value);
            }

            [Fact]
            public async void GivenPurchaseDateNotProvided_SaveAsync_SetsParameterToDbNull()
            {
                var sut = new MicrosoftSqlServerPropertyDataStore(Connection, Logger);
                var property = new Property(new PropertyType(1, "Apartment"), new Address("line 1", "line 2", "City", "County", "postcode"), null, null, true, 2, "some notes");
                Connection.ExecuteScalarAsync(Arg.Any<string>(),
                    Arg.Is<List<SqlParameter>>(parameters => parameters[2].ParameterName == "@purchase_date" && parameters[2].Value == DBNull.Value)).Returns(2);
                var result = await sut.SaveAsync(property);

                var savedProperty = Assert.IsType<Property>(result.Value);
                Assert.Equal(2, savedProperty.Id.Value);
            }

            [Fact]
            public async void GivenParkingSpacesNotProvided_SaveAsync_SetsParameterToDbNull()
            {
                var sut = new MicrosoftSqlServerPropertyDataStore(Connection, Logger);
                var property = new Property(new PropertyType(1, "Apartment"), new Address("line 1", "line 2", "City", "County", "postcode"), null, new DateOnly(2023, 02, 06), true, null, "some notes");
                Connection.ExecuteScalarAsync(Arg.Any<string>(),
                    Arg.Is<List<SqlParameter>>(parameters => parameters[4].ParameterName == "@parking_spaces" && parameters[4].Value == DBNull.Value)).Returns(2);
                var result = await sut.SaveAsync(property);

                var savedProperty = Assert.IsType<Property>(result.Value);
                Assert.Equal(2, savedProperty.Id.Value);
            }

            [Fact]
            public async void GivenNotesNotProvided_SaveAsync_SetsParameterToDbNull()
            {
                var sut = new MicrosoftSqlServerPropertyDataStore(Connection, Logger);
                var property = new Property(new PropertyType(1, "Apartment"), new Address("line 1", "line 2", "City", "County", "postcode"), null, new DateOnly(2023, 02, 06), true, 2, string.Empty);
                Connection.ExecuteScalarAsync(Arg.Any<string>(),
                    Arg.Is<List<SqlParameter>>(parameters => parameters[5].ParameterName == "@notes" && parameters[5].Value == DBNull.Value)).Returns(2);
                var result = await sut.SaveAsync(property);

                var savedProperty = Assert.IsType<Property>(result.Value);
                Assert.Equal(2, savedProperty.Id.Value);
            }

            [Fact]
            public async void GivenAddressLine2NotProvided_SaveAsync_SetsParameterToDbNull()
            {
                var sut = new MicrosoftSqlServerPropertyDataStore(Connection, Logger);
                var property = new Property(new PropertyType(1, "Apartment"), new Address("line 1", string.Empty, "City", "County", "postcode"), null, new DateOnly(2023, 02, 06), true, 2, "some notes");
                Connection.ExecuteScalarAsync(Arg.Any<string>(),
                    Arg.Is<List<SqlParameter>>(parameters => parameters[7].ParameterName == "@line2" && parameters[7].Value == DBNull.Value)).Returns(2);
                var result = await sut.SaveAsync(property);

                var savedProperty = Assert.IsType<Property>(result.Value);
                Assert.Equal(2, savedProperty.Id.Value);
            }

            [Fact]
            public async void GivenRegionNotProvided_SaveAsync_SetsParameterToDbNull()
            {
                var sut = new MicrosoftSqlServerPropertyDataStore(Connection, Logger);
                var property = new Property(new PropertyType(1, "Apartment"), new Address("line 1", "line 2", "City", string.Empty, "postcode"), null, new DateOnly(2023, 02, 06), true, 2, "some notes");
                Connection.ExecuteScalarAsync(Arg.Any<string>(),
                    Arg.Is<List<SqlParameter>>(parameters => parameters[9].ParameterName == "@region" && parameters[9].Value == DBNull.Value)).Returns(2);
                var result = await sut.SaveAsync(property);

                var savedProperty = Assert.IsType<Property>(result.Value);
                Assert.Equal(2, savedProperty.Id.Value);
            }

            [Fact]
            public async void GivenExistingPropertyToSave_SaveAsync_SavesProperty()
            {
                var sut = new MicrosoftSqlServerPropertyDataStore(Connection, Logger);
                var property = new Property(2, new PropertyType(1, "Apartment"), new Address("line 1", "line 2", "City", "County", "postcode"), 100000, new DateOnly(2023, 02, 06), true, 2, "some notes");

                Connection.ExecuteScalarAsync("UPDATE property.overview SET type = @type, purchase_price = @purchase_price, purchase_date = @purchase_date, garage = @garage, parking_spaces = @parking_spaces, notes = @notes WHERE id = @id;UPDATE property.address SET line1 = @line1, line2 = @line2, city = @city, region = @region, postcode = @postcode where id = @id;SELECT @id",
                    Arg.Is<List<SqlParameter>>(parameters =>
                parameters[0].ParameterName == "@type" && (int)parameters[0].Value == property.Type.Id &&
                parameters[1].ParameterName == "@purchase_price" && (decimal)parameters[1].Value == property.PurchasePrice &&
                parameters[2].ParameterName == "@purchase_date" && (DateOnly)parameters[2].Value == property.PurchaseDate &&
                parameters[3].ParameterName == "@garage" && (bool)parameters[3].Value == property.Garage &&
                parameters[4].ParameterName == "@parking_spaces" && (byte)parameters[4].Value == property.NumberOfParkingSpaces &&
                parameters[5].ParameterName == "@notes" && (string)parameters[5].Value == property.Notes &&
                parameters[6].ParameterName == "@line1" && (string)parameters[6].Value == property.Address.AddressLine1 &&
                parameters[7].ParameterName == "@line2" && (string)parameters[7].Value == property.Address.AddressLine2 &&
                parameters[8].ParameterName == "@city" && (string)parameters[8].Value == property.Address.City &&
                parameters[9].ParameterName == "@region" && (string)parameters[9].Value == property.Address.Region &&
                parameters[10].ParameterName == "@postcode" && (string)parameters[10].Value == property.Address.PostalCode &&
                parameters[11].ParameterName == "@id" && (int)parameters[11].Value == 2
                )).Returns(2);
                var result = await sut.SaveAsync(property);

                var savedProperty = Assert.IsType<Property>(result.Value);
                Assert.Equal(2, savedProperty.Id.Value);
            }
        }

        public class DeleteAsync : Fixture
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-2)]
            public void GivenInvalidId_DeleteAsync_ThrowsArgumentOutOfRangeException(int id)
            {
                var sut = new MicrosoftSqlServerPropertyDataStore(Connection, Logger);
                Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await sut.DeleteAsync(id));
            }

            [Fact]
            public async void GivenRecordExists_DeleteAsync_ReturnsTrue()
            {
                var sut = new MicrosoftSqlServerPropertyDataStore(Connection, Logger);

                Connection.ExecuteCommandAsync("DELETE FROM property.overview WHERE id = @id", Arg.Is<List<SqlParameter>>(parameters => parameters[0].ParameterName == "@id" && (int)parameters[0].Value == 2)).Returns(1);
                var result = await sut.DeleteAsync(2);
                Assert.True(result);
            }

            [Fact]
            public async void GivenRecordDoesNotExist_DeleteAsync_ReturnsTrue()
            {
                var sut = new MicrosoftSqlServerPropertyDataStore(Connection, Logger);

                Connection.ExecuteCommandAsync("DELETE FROM property.overview WHERE id = @id", Arg.Is<List<SqlParameter>>(parameters => parameters[0].ParameterName == "@id" && (int)parameters[0].Value == 2)).Returns(0);
                var result = await sut.DeleteAsync(2);
                Assert.False(result);
            }
        }

        public class GetAsync : Fixture
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-2)]
            public void GivenInvalidId_GetAsync_ThrowsArgumentOutOfRangeException(int id)
            {
                var sut = new MicrosoftSqlServerPropertyDataStore(Connection, Logger);
                Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await sut.GetAsync(id));
            }

            [Fact]
            public async void GivenRecordNotFound_GetAsync_ReturnsNull()
            {
                var sut = new MicrosoftSqlServerPropertyDataStore(Connection, Logger);
                Connection.GetSingleRowAsync("SELECT purchase_price, type.id, type.name, purchase_date, garage, parking_spaces, notes, property.address.line1, property.address.line2, property.address.city, property.address.region, property.address.postcode FROM property.overview INNER JOIN property.type ON property.type.id = property.overview.type LEFT JOIN property.address ON property.address.id = property.overview.id WHERE property.overview.id = @id", 2, Arg.Any<Func<dynamic, object>>()).ReturnsNull();
                var result = await sut.GetAsync(2);
                Assert.Null(result);
            }

            [Fact]
            public async void GivenRecordFound_GetAsync_ReturnsProperty()
            {
                var sut = new MicrosoftSqlServerPropertyDataStore(Connection, Logger);

                Connection.GetSingleRowAsync("SELECT purchase_price, type.id, type.name, purchase_date, garage, parking_spaces, notes, property.address.line1, property.address.line2, property.address.city, property.address.region, property.address.postcode FROM property.overview INNER JOIN property.type ON property.type.id = property.overview.type LEFT JOIN property.address ON property.address.id = property.overview.id WHERE property.overview.id = @id", 2, Arg.Any<Func<dynamic, object>>()).Returns(new Property(2, new PropertyType(1, "Apartment"), new Address("line 1", "line 2", "city", "region", "postcode"), 100000, new DateOnly(2023, 02, 06), true, 3, "some notes"));

                var result = await sut.GetAsync(2);

                var property = Assert.IsType<Property>(result);
                Assert.Equal(2, property.Id);
                Assert.Equal(1, property.Type.Id);
                Assert.Equal(100000, property.PurchasePrice);
                Assert.Equal(new DateOnly(2023, 02, 06), property.PurchaseDate);
                Assert.True(property.Garage);
                Assert.Equal((byte)3, property.NumberOfParkingSpaces);
                Assert.Equal("some notes", property.Notes);
                Assert.Equal("line 1", property.Address.AddressLine1);
                Assert.Equal("line 2", property.Address.AddressLine2);
                Assert.Equal("city", property.Address.City);
                Assert.Equal("region", property.Address.Region);
                Assert.Equal("postcode", property.Address.PostalCode);
            }
        }
        public class Fixture
        {
            public IDatabaseConnection Connection { get; }
            public ILogger<MicrosoftSqlServerPropertyDataStore> Logger;
            public string ConnectionString { get; }

            public Fixture()
            {
                Connection = Substitute.For<IDatabaseConnection>();
                Logger = Substitute.For<ILogger<MicrosoftSqlServerPropertyDataStore>>();
                ConnectionString = "Server=localhost;Database=propertyManager;Trusted_Connection=False;MultipleActiveResultSets=true;Integrated Security=true;TrustServerCertificate=True";
            }
        }
    }
}
