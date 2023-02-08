namespace ServiceTests.propertyType.DataStores
{
    using DataAccess.database;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using NSubstitute.ReturnsExtensions;
    using Service.propertyType;
    using Service.propertyType.DataStores;

    public class SqlPropertyTypeDataStoreTests
    {
        public class Ctor : Fixture
        {
            [Fact]
            public void GivenNullDatabaseConnection_Ctor_ThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(() => new MicrosoftSqlServerPropertyTypeDataStore(null, Logger));
            }

            [Fact]
            public void GivenNullLogger_Ctor_ThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(() => new MicrosoftSqlServerPropertyTypeDataStore(Connection, null));
            }
        }

        public class DeleteAsync : Fixture
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-2)]
            public void GivenInvalidId_DeleteAsync_ThrowsArgumentOutOfRangeException(int id)
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(Connection, Logger);
                Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await sut.DeleteAsync(id));
            }

            [Fact]
            public async void GivenRecordExists_DeleteAsync_ReturnsTrue()
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(Connection, Logger);
                Connection.ExecuteCommandAsync("DELETE FROM property.type WHERE id = @id", Arg.Is<List<SqlParameter>>((parameters) => parameters[0].ParameterName == "@id" && (int)parameters[0].Value == 2)).Returns(1);
                var result = await sut.DeleteAsync(2);
                Assert.True(result);
            }

            [Fact]
            public async void GivenRecordDoesNotExist_DeleteAsync_ReturnsFalse()
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(Connection, Logger);
                Connection.ExecuteCommandAsync("DELETE FROM property.type WHERE id = @id", Arg.Is<List<SqlParameter>>((parameters) => parameters[0].ParameterName == "@id" && (int)parameters[0].Value == 2)).Returns(0);
                var result = await sut.DeleteAsync(2);
                Assert.False(result);
            }
        }

        public class SaveAsync : Fixture
        {
            [Fact]
            public void GivenNullPropertyType_SaveAsync_ThrowsArgumentNullException()
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(Connection, Logger);
                Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.SaveAsync(null));
            }

            [Fact]
            public async void GivenNewPropertyTypeToSave_SaveAsync_SavesProperty()
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(Connection, Logger);
                var propertyType = new PropertyType("name");

                Connection.ExecuteScalarAsync("INSERT INTO property.type (name) VALUES (@name);SELECT scope_identity()", Arg.Is<List<SqlParameter>>((parameters) => parameters[0].ParameterName == "@name" && (string)parameters[0].Value == "name")).Returns(2);
                var result = await sut.SaveAsync(propertyType);

                var savedPropertyType = Assert.IsType<PropertyType>(result);
                Assert.Equal(2, savedPropertyType.Id);
                Assert.Equal("name", savedPropertyType.Name);
            }

            [Fact]
            public async void GivenNewPropertyTypeToSaveFails_SaveAsync_ReturnsNull()
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(Connection, Logger);
                var propertyType = new PropertyType("name");

                Connection.ExecuteScalarAsync("INSERT INTO property.type (name) VALUES (@name);SELECT scope_identity()", Arg.Is<List<SqlParameter>>((parameters) => parameters[0].ParameterName == "@name" && (string)parameters[0].Value == "name")).ReturnsNull();
                var result = await sut.SaveAsync(propertyType);
                Assert.Null(result);
            }

            [Theory]
            [InlineData(0)]
            [InlineData(-2)]
            public void GivenNewPropertyTypeToSaveFailsWithInvalidId_SaveAsync_ThrowsArgumentOutOfRangeException(int id)
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(Connection, Logger);
                var propertyType = new PropertyType("name");

                Connection.ExecuteScalarAsync("INSERT INTO property.type (name) VALUES (@name);SELECT scope_identity()", Arg.Is<List<SqlParameter>>((parameters) => parameters[0].ParameterName == "@name" && (string)parameters[0].Value == "name")).ReturnsNull();
                Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await sut.SaveAsync(propertyType));
            }

            [Fact]
            public async void GivenExistingPropertyTypeToSave_SaveAsync_SavesProperty()
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(Connection, Logger);
                var propertyType = new PropertyType(2, "name");

                Connection.ExecuteScalarAsync("UPDATE property.type SET name = @name where id = @id", Arg.Is<List<SqlParameter>>((parameters) => parameters[0].ParameterName == "@name" && (string)parameters[0].Value == "name" &&
                    parameters[1].ParameterName == "@id" && (int)parameters[1].Value == 2
                )).Returns(1);

                var result = await sut.SaveAsync(propertyType);

                var savedPropertyType = Assert.IsType<PropertyType>(result);
            }
        }

        public class GetAsync : Fixture
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-2)]
            public void GivenInvalidId_GetAsync_ThrowsArgumentOutOfRangeException(int id)
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(Connection, Logger);
                Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await sut.GetAsync(id));
            }

            [Fact]
            public async void GivenRecordExists_GetAsync_ReturnsRecord()
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(Connection, Logger);
                Connection.GetSingleRowAsync("SELECT id, name FROM property.type WHERE id = @id", 3, Arg.Any<Func<dynamic, object>>()).Returns(new PropertyType(3, "name"));
                var result = await sut.GetAsync(3);

                var propertyType = Assert.IsType<PropertyType>(result);
                Assert.Equal(3, propertyType.Id);
                Assert.Equal("name", propertyType.Name);
            }

            [Fact]
            public async void GivenRecordDoesNotExist_GetAsync_ReturnsNull()
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(Connection, Logger);
                Connection.GetSingleRowAsync("SELECT id, name FROM property.type WHERE id = @id", 3, Arg.Any<Func<dynamic, object>>()).ReturnsNull();
                var result = await sut.GetAsync(3);
                Assert.Null(result);
            }
        }

        public class Fixture
        {
            public IDatabaseConnection Connection;
            public ILogger<MicrosoftSqlServerPropertyTypeDataStore> Logger;

            public Fixture()
            {
                Connection = Substitute.For<IDatabaseConnection>();
                Logger = Substitute.For<ILogger<MicrosoftSqlServerPropertyTypeDataStore>>();
            }
        }
    }
}
