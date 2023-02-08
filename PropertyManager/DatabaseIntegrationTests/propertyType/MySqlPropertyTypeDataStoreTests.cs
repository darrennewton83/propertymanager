namespace DatabaseIntegrationTests.propertyType
{
    using DatabaseIntegrationTests.DatabaseFixtures;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using Service.propertyType;
    using Service.propertyType.DataStores;

    public class MySqlPropertyTypeDataStoreTests
    {
        public class SaveAsync : Fixture
        {
            [Fact]
            public async void GivenNewPropertyTypeToSave_SaveAsync_SavesPropertyType()
            {
                var sut = new MySqlPropertyTypeDataStore(new DataAccess.database.MySql(this.DatabaseLogger, this.ConnectionString), Logger);

                var propertyType = new PropertyType("name");
                var result = await sut.SaveAsync(propertyType);

                var savedPropertyType = Assert.IsType<PropertyType>(result);
                Assert.True(savedPropertyType.Id.HasValue);
            }
        }

        public class GetAsync : Fixture
        {
            [Fact]
            public async void GivenValidId_GetAsync_ReturnsPropertyType()
            {
                var sut = new MySqlPropertyTypeDataStore(new DataAccess.database.MySql(this.DatabaseLogger, this.ConnectionString), Logger);
                var propertyType = await sut.GetAsync(3);

                Assert.IsType<PropertyType>(propertyType);
                Assert.Equal(3, propertyType.Id);
                Assert.Equal("Semi Detached House", propertyType.Name);
            }

            [Fact]
            public async void GivenInValidId_GetAsync_ReturnsNull()
            {
                var sut = new MySqlPropertyTypeDataStore(new DataAccess.database.MySql(this.DatabaseLogger, this.ConnectionString), Logger);
                var result = await sut.GetAsync(999);

                Assert.Null(result);
            }
        }

        public class DeleteAsync : Fixture
        {
            [Fact]
            public async void GivenValidId_DeleteAsync_ReturnsTrue()
            {
                var sut = new MySqlPropertyTypeDataStore(new DataAccess.database.MySql(this.DatabaseLogger, this.ConnectionString), Logger);
                var result = await sut.DeleteAsync(3);
                Assert.True(result);
            }

            [Fact]
            public async void GivenInValidId_DeleteAsync_ReturnsFalse()
            {
                var sut = new MySqlPropertyTypeDataStore(new DataAccess.database.MySql(this.DatabaseLogger, this.ConnectionString), Logger);
                var result = await sut.DeleteAsync(999);
                Assert.False(result);
            }
        }

        public class Fixture : MySqlDatabaseFixture
        {
            public ILogger<DataAccess.database.MySql> DatabaseLogger;
            public ILogger<MicrosoftSqlServerPropertyTypeDataStore> Logger;

            public Fixture()
            {
                DatabaseLogger = Substitute.For<ILogger<DataAccess.database.MySql>>();
                Logger = Substitute.For<ILogger<MicrosoftSqlServerPropertyTypeDataStore>>();
            }
        }
    }
}
