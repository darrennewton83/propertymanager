namespace PropertyManager.Shared.UnitTests.Address
{
    using PropertyManager.Shared.Address;

    public class AddressTests
    {
        [Fact]
        public void Ctor_Valid_SetsProperties()
        {
            var address = new Address("line 1", "line 2", "city", "region", "postal code");

            Assert.Equal("line 1", address.AddressLine1);
            Assert.Equal("line 2", address.AddressLine2);
            Assert.Equal("city", address.City);
            Assert.Equal("region", address.Region);
            Assert.Equal("postal code", address.PostalCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GivenEmptyAddressLine1_Ctor_ThrowsArgumentNullException(string address1)
        {
            Assert.Throws<ArgumentNullException>(() => new Address(address1, "line 2", "city", "region", "postal code"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GivenEmptyCity_Ctor_ThrowsArgumentNullException(string city)
        {
            Assert.Throws<ArgumentNullException>(() => new Address("Line 1", "line 2", city, "region", "postal code"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GivenEmptyPostalCode_Ctor_ThrowsArgumentNullException(string postcode)
        {
            Assert.Throws<ArgumentNullException>(() => new Address("Line 1", "line 2", "City", "region", postcode));
        }
    }
}
