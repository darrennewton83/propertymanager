using Service.propertyType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTests.propertyType
{
    public class PropertyTypeTests
    {
        [Fact]
        public void GivenValid_Ctor_SetsProperties()
        {
            var sut = new PropertyType(1, "name");
            Assert.Equal(1, sut.Id);
            Assert.Equal("name", sut.Name);
        }

        [Fact]
        public void GivenValid_Ctor1_SetsProperties()
        {
            var sut = new PropertyType("name");
            Assert.Equal("name", sut.Name);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void GivenInvalidId_Ctor_ThrowsArgumentOutOfRangeException(int id)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PropertyType(id, "name"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GivenInvalidName_Ctor_ThrowsArgumentNullException(string name)
        {
            Assert.Throws<ArgumentNullException>(() => new PropertyType(name));
        }
    }
}
