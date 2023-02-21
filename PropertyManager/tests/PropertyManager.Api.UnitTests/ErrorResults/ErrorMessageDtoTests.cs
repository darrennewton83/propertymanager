namespace PropertyManager.Api.UnitTests.ErrorResults
{
    using PropertyManager.Api.ErrorResults;

    public class ErrorMessageDtoTests
    {
        [Fact]
        public void GivenValid_Ctor_SetsProperties()
        {
            var sut = new ErrorMessageDto()
            {
                Message = "a message"
            };
            
            Assert.Equal("a message", sut.Message);
        }
    }
}
