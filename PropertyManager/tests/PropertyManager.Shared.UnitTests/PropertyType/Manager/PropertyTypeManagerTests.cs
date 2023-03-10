namespace PropertyManager.Shared.UnitTests.PropertyType.Manager
{
    using PropertyManager.Shared.PropertyType;
    using PropertyManager.Shared.PropertyType.DataStores;
    using PropertyManager.Shared.PropertyType.Manager;
    using NSubstitute;
    using NSubstitute.ReturnsExtensions;
    using PropertyManager.Shared.EntityResults;
    using Microsoft.Extensions.Logging;

    public class PropertyTypeManagerTests
    {
        public class Ctor : Fixture
        {
            [Fact]
            public void GivenNullDataStore_Ctor_ThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(() => new PropertyTypeManager(null, Logger));
            }

            [Fact]
            public void GivenNullLogger_Ctor_ThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(() => new PropertyTypeManager(DataStore, null));
            }
        }

        public class DeleteAsync : Fixture
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-2)]
            public void GivenInvalidId_DeleteAsync_ThrowsArgumentOutOfRangeException(int id)
            {
                var sut = new PropertyTypeManager(DataStore, Logger);
                Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await sut.DeleteAsync(id));
            }

            [Fact]
            public async void GivenRecordExists_DeleteAsync_ReturnsTrue()
            {
                var sut = new PropertyTypeManager(DataStore, Logger);
                DataStore.DeleteAsync(3).Returns(true);
                var result = await sut.DeleteAsync(3);
                Assert.True(result);
            }

            [Fact]
            public async void GivenRecordDoesNotExist_DeleteAsync_ReturnsFalse()
            {
                var sut = new PropertyTypeManager(DataStore, Logger);
                DataStore.DeleteAsync(3).Returns(false);
                var result = await sut.DeleteAsync(3);
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
                var sut = new PropertyTypeManager(DataStore, Logger);
                Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await sut.GetAsync(id));
            }

            [Fact]
            public async void GivenRecordExists_GetAsync_ReturnsRecord()
            {
                var sut = new PropertyTypeManager(DataStore, Logger);
                DataStore.GetAsync(3).Returns(new PropertyType(3, "name"));
                var result = await sut.GetAsync(3);

                Assert.IsType<PropertyType>(result);
            }

            [Fact]
            public async void GivenRecordDoesNotExist_GetAsync_ReturnsNull()
            {
                var sut = new PropertyTypeManager(DataStore, Logger);
                DataStore.GetAsync(3).ReturnsNull();
                var result = await sut.GetAsync(3);
                Assert.Null(result);
            }
        }

        public class GetAllAsync : Fixture
        {
            

            [Fact]
            public async void GivenRecordsExist_GetAsync_ReturnsRecords()
            {
                var sut = new PropertyTypeManager(DataStore, Logger);
                DataStore.GetAsync().Returns(new List<IPropertyType> { new PropertyType(3, "Apartment"), new PropertyType(1, "House") });
                var result = await sut.GetAsync();

                var propertyTypes = Assert.IsAssignableFrom<IEnumerable<IPropertyType>>(result);

                Assert.Collection(propertyTypes, element1 => { Assert.Equal(3, element1.Id); Assert.Equal("Apartment", element1.Name); },
                    element2 => { Assert.Equal(1, element2.Id); Assert.Equal("House", element2.Name); }
                    );
            }

            [Fact]
            public async void GivenRecordDoesNotExist_GetAsync_ReturnsNull()
            {
                var sut = new PropertyTypeManager(DataStore, Logger);
                DataStore.GetAsync().Returns(Enumerable.Empty<IPropertyType>());
                var result = await sut.GetAsync();
                Assert.Empty(result);
            }
        }

        public class SaveAsync : Fixture
        {
            [Fact]
            public void GivenNullPropertyType_SaveAsync_ThrowsArgumentNullException()
            {
                var sut = new PropertyTypeManager(DataStore, Logger);
                Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.SaveAsync(null));
            }

            [Fact]
            public async void GivenNewPropertyTypeToSave_SaveAsync_SavesPropertyType()
            {
                var sut = new PropertyTypeManager(DataStore, Logger);
                var propertyType = new PropertyType("name");
                DataStore.SaveAsync(Arg.Any<PropertyType>()).Returns(new ValueResult<IPropertyType>(propertyType));

                var result = await sut.SaveAsync(propertyType);
                Assert.NotNull(result);
            }

            [Fact]
            public async void GivenExistingPropertyTypeToSave_SaveAsync_SavesPropertyType()
            {
                var sut = new PropertyTypeManager(DataStore, Logger);
                var propertyType = new PropertyType(3, "name");
                DataStore.SaveAsync(Arg.Any<PropertyType>()).Returns(new ValueResult<IPropertyType>(propertyType));

                var result = await sut.SaveAsync(propertyType);
                Assert.NotNull(result);
            }

            [Fact]
            public async void GivenPropertyTypeToSaveFails_SaveAsync_ReturnsErrorResult()
            {
                var sut = new PropertyTypeManager(DataStore, Logger);
                var propertyType = new PropertyType(3, "name");
                DataStore.SaveAsync(Arg.Any<PropertyType>()).Returns(new EntityErrorResult<IPropertyType>());

                var result = await sut.SaveAsync(propertyType);
                Assert.Equal(ResultType.Error, result.Type);
            }
        }

        public class Fixture
        {
            public IPropertyTypeDataStore DataStore { get; }
            public ILogger<PropertyTypeManager> Logger { get; }

            public Fixture()
            {
                DataStore = Substitute.For<IPropertyTypeDataStore>();
                Logger = Substitute.For<ILogger<PropertyTypeManager>>();
            }
        }
    }
}
