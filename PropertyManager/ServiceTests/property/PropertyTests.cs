using Service.address;
using Service.property;
using Service.propertyType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTests.property
{
    public class PropertyTests : Fixture
    {
        [Fact]
        public void Ctor_Valid_SetsProperties()
        {
            var sut = new Property(4, PropertyType, this.SampleAddress, 100000, new DateOnly(2023, 01, 17), true, 2, "some notes");
            Assert.Equal(4, sut.Id);
            Assert.Equal(PropertyType, sut.Type);
            Assert.Equal(this.SampleAddress, sut.Address);
            Assert.Equal(100000, sut.PurchasePrice);
            Assert.Equal(new DateOnly(2023, 01, 17), sut.PurchaseDate);
            Assert.True(sut.Garage);
            Assert.Equal(2, sut.NumberOfParkingSpaces.Value);
            Assert.Equal("some notes", sut.Notes);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void GivenInvalidId_Ctor_ThrowsArgumentOutOfRangeException(int id)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Property(id, PropertyType, this.SampleAddress, null, null, false, null, ""));
        }

        [Fact]
        public void GivenNullAddress_Ctor_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Property(1, PropertyType, null, null, null, false, null, ""));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void GivenPurchasePriceOutOfRange_Ctor_ThrowsArgumentOutOfRangeException(decimal purchasePrice)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Property(1, PropertyType, this.SampleAddress, purchasePrice, null, false, null, ""));
        }

        [Fact]
        public void GivenPurchasePriceNotSet_Ctor_ThrowsArgumentOutOfRangeException()
        {
            var sut =  new Property(1, PropertyType, this.SampleAddress, null, null, false, null, "");
            Assert.False(sut.PurchasePrice.HasValue);
        }
    }

    public class Fixture
    {
        public IAddress SampleAddress { get; }
        public PropertyType PropertyType { get; }

        public Fixture() 
        {
            this.SampleAddress = new Address("address line 1", "address line 2", "city", "region", "postal code");
            this.PropertyType = new PropertyType(1, "Apartment");
        }
    }
}
