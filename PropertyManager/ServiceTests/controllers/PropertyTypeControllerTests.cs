namespace ServiceTests.controllers
{
    using AutoMapper;
    using NSubstitute;
    using Service.Controllers;
    using Service.propertyType.Manager;

    public class PropertyTypeControllerTests
    {
        [Fact]
        public void GivenNullPropertyManager_Ctor_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PropertyTypeController(null, Substitute.For<IMapper>()));
        }

        [Fact]
        public void GivenNullMapper_Ctor_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PropertyTypeController(Substitute.For<IPropertyTypeManager>(), null));
        }
    }
}
