namespace ServiceTests.EntityResults
{
    using NSubstitute;
    using Service.EntityResults;
    using Service.propertyType;

    public class ValueResultTests
    {
        [Fact]
        public void GivenValid_Ctor_SetsProperties()
        {
            var sut = new ValueResult<IPropertyType>(Substitute.For<IPropertyType>());
            Assert.Equal(ResultType.Success, sut.Type);
            Assert.NotNull(sut.Value);
            Assert.IsAssignableFrom<IPropertyType>(sut.Value);
        }

        [Fact]
        public void GivenValueNull_Ctor_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ValueResult<IPropertyType>(null));
        }
    }
}
