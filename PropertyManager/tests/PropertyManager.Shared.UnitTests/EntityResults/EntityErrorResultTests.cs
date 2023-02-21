namespace PropertyManager.Shared.UnitTests.EntityResults
{
    using PropertyManager.Shared.EntityResults;
    using PropertyManager.Shared.PropertyType;

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
