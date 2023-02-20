namespace ServiceTests.EntityResults
{
    using Service.EntityResults;
    using Service.propertyType;

    public class EntityErrorResultTests
    {
        [Fact]
        public void GivenValid_Ctor_SetsProperties()
        {
            var sut = new EntityErrorResult<PropertyType>();
            Assert.Equal(ResultType.Error, sut.Type);
        }
    }
}
