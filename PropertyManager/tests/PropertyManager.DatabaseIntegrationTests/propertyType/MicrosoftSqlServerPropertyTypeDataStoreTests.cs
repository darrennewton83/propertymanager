namespace PropertyManager.Shared.DatabaseIntegrationTests.propertyType
{
    using DataAccess.Database;
    using DatabaseIntegrationTests.DatabaseFixtures;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using PropertyManager.Shared.PropertyType;
    using PropertyManager.Shared.PropertyType.DataStores;

    [Collection("DatabaseCollection")]
    public class MicrosoftSqlServerPropertyTypeDataStoreTests
    {
        [Collection("DatabaseCollection")]
        public class SaveAsync : Fixture
        {
            

            public SaveAsync(SqlServerDatabaseFixture databaseFixture) : base (databaseFixture)
            {
            }

            [Fact]
            public async void GivenNewPropertyTypeToSave_SaveAsync_SavesProperty()
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(new MicrosoftSqlServer(this.DatabaseLogger, DatabaseFixture.ConnectionString), Logger);

                var propertyType = new PropertyType("name");
                var result = await sut.SaveAsync(propertyType);

                var savedPropertyType = Assert.IsType<PropertyType>(result.Value);
                Assert.True(savedPropertyType.Id.HasValue);
            }
        }

        [Collection("DatabaseCollection")]
        public class GetAsync : Fixture
        {
            public GetAsync(SqlServerDatabaseFixture databaseFixture) : base (databaseFixture)
            {

            }

            [Fact]
            public async void GivenValidId_GetAsync_ReturnsPropertyType()
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(new MicrosoftSqlServer(this.DatabaseLogger, DatabaseFixture.ConnectionString), Logger);
                var propertyType = await sut.GetAsync(2);

                Assert.IsType<PropertyType>(propertyType);
                Assert.Equal(2, propertyType.Id);
                Assert.Equal("Detached House", propertyType.Name);
            }

            [Fact]
            public async void GivenInValidId_GetAsync_ReturnsNull()
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(new MicrosoftSqlServer(this.DatabaseLogger, DatabaseFixture.ConnectionString), Logger);
                var result = await sut.GetAsync(999);

                Assert.Null(result);
            }
        }

        [Collection("DatabaseCollection")]
        public class GetAllAsync : Fixture
        {
            public GetAllAsync(SqlServerDatabaseFixture databaseFixture) : base(databaseFixture)
            {
            }

            [Fact]
            public async void GivenRecordsExist_GetAsync_ReturnsPropertyTypes()
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(new MicrosoftSqlServer(this.DatabaseLogger, DatabaseFixture.ConnectionString), Logger);
                var result = await sut.GetAsync();

                var propertyTypes = Assert.IsAssignableFrom<IEnumerable<IPropertyType>>(result);

                Assert.Collection(propertyTypes,
                    element1 => { Assert.Equal("Apartment", element1.Name); Assert.Equal(1, element1.Id); },
                element2 => { Assert.Equal("Detached House", element2.Name); Assert.Equal(2, element2.Id); },
                element3 => { Assert.Equal("Semi Detached House", element3.Name); Assert.Equal(3, element3.Id); },
                element4 => { Assert.Equal("Studio Flat", element4.Name); Assert.Equal(5, element4.Id); },
                element5 => { Assert.Equal("Terraced House", element5.Name); Assert.Equal(4, element5.Id); },
                element6 => { Assert.Equal("name", element6.Name); }
                );
            }
        }

        [Collection("DatabaseCollection")]
        public class DeleteAsync : Fixture
        {
            public DeleteAsync(SqlServerDatabaseFixture databaseFixture) : base(databaseFixture)
            {
                DatabaseFixture = databaseFixture; 
            }

            [Fact]
            public async void GivenValidId_DeleteAsync_ReturnsTrue()
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(new MicrosoftSqlServer(this.DatabaseLogger, DatabaseFixture.ConnectionString), Logger);
                var result = await sut.DeleteAsync(3);
                Assert.True(result);
            }

            [Fact]
            public async void GivenInValidId_DeleteAsync_ReturnsFalse()
            {
                var sut = new MicrosoftSqlServerPropertyTypeDataStore(new MicrosoftSqlServer(this.DatabaseLogger, DatabaseFixture.ConnectionString), Logger);
                var result = await sut.DeleteAsync(999);
                Assert.False(result);
            }
        }

        public class Fixture
        {
            public ILogger<MicrosoftSqlServer> DatabaseLogger;
            public ILogger<MicrosoftSqlServerPropertyTypeDataStore> Logger;
            public SqlServerDatabaseFixture DatabaseFixture;
            

            public Fixture(SqlServerDatabaseFixture databaseFixture)
            {
                DatabaseLogger = Substitute.For<ILogger<MicrosoftSqlServer>>();
                Logger = Substitute.For<ILogger<MicrosoftSqlServerPropertyTypeDataStore>>();
                DatabaseFixture = databaseFixture;
            }
        }
    }
}
