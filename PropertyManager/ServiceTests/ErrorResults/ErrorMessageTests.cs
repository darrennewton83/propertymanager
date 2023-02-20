namespace ServiceTests.ErrorResults
{
    using Service.ErrorResults;

    public class ErrorMessageTests
    {
        [Fact]
        public void GivenValid_Ctor_SetsProperties()
        {
            var sut = new ErrorMessage("a message");
            Assert.Equal("a message", sut.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GivenEmptyMessage_Ctor_ThrowsArgumentNullException(string message)
        {
            Assert.Throws<ArgumentNullException>(() => new ErrorMessage(message));
        }
    }
}
