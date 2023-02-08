using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Service.address;
using Service.property;
using Service.property.DataStores;
using Service.property.Manager;
using Service.propertyType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ServiceTests.property.Manager
{
    public class PropertyManagerTests
    {
        public class Ctor
        {
            [Fact]
            public void Ctor_Valid_SetsProperties()
            {
                var sut = new PropertyManager(Substitute.For<IPropertyDataStore>());
                Assert.NotNull(sut);
            }

            [Fact]
            public void Ctor_NullDataStore_ThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(() => new PropertyManager(null));
            }

        }

        public class DeleteAsync : Fixture
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-2)]
            public void GivenInvalidId_DeleteAsync_ThrowsArgumentOutOfRangeException(int id)
            {
                var sut = new PropertyManager(Substitute.For<IPropertyDataStore>());
                Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await sut.DeleteAsync(id));
            }

            [Fact]
            public async void GivenIdExists_DeleteAsync_ReturnsTrue()
            {
                var sut = new PropertyManager(PropertyDataStore);
                PropertyDataStore.DeleteAsync(2).Returns(true);
                var result = await sut.DeleteAsync(2);

                Assert.True(result);
            }

            [Fact]
            public async void GivenIdDoesNotExist_DeleteAsync_ReturnsFalse()
            {
                var sut = new PropertyManager(PropertyDataStore);
                PropertyDataStore.DeleteAsync(2).Returns(false);
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
                var sut = new PropertyManager(PropertyDataStore);
                Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await sut.GetAsync(id));
            }

            [Fact]
            public async void GivenIExists_GetAsync_ReturnsNull()
            {
                var sut = new PropertyManager(PropertyDataStore);
                PropertyDataStore.GetAsync(2).Returns(new Property(2, PropertyType, new Address("line 1", "line 2", "city", "region", "postcode"), 100000, new DateOnly(2023, 02, 06), true, 2, "notes"));
                var result = await sut.GetAsync(2);
                Assert.IsType<Property>(result);
            }

            [Fact]
            public async void GivenIdDoesNotExist_GetAsync_ReturnsNull()
            {
                var sut = new PropertyManager(PropertyDataStore);
                PropertyDataStore.GetAsync(2).ReturnsNull();
                var result = await sut.GetAsync(2);
                Assert.Null(result);
            }
        }

        public class Fixture
        {
            public IPropertyDataStore PropertyDataStore;
            public PropertyType PropertyType;
            public Fixture()
            {
                PropertyDataStore = Substitute.For<IPropertyDataStore>();
                PropertyType = new PropertyType(1, "Apartment");
            }
        }
    }
}
