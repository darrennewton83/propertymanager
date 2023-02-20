namespace ServiceTests.ErrorResults
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using NSubstitute;
    using Service.ErrorResults;

    public class ErrorResultTests
    {
        public class Ctor
        {
            [Theory]
            [InlineData("")]
            [InlineData(" ")]
            [InlineData(null)]
            public void GivenNullErrorMessage_Ctor_ThrowsArgumentNullException(string message)
            {
                Assert.Throws<ArgumentNullException>(() => new ErrorResult(message, Substitute.For<IMapper>()));
            }

            [Fact]
            public void GivenNullMapper_Ctor_ThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(() => new ErrorResult("a message", null));
            }
        }

        public class ExecuteResult : Fixture
        {
            [Fact]
            public void GivenNullContenxt_ExecuteResult_ThrowsArgumentNullException()
            {
                var sut = new ErrorResult(Message, Mapper);

                Assert.Throws<ArgumentNullException>(() => sut.ExecuteResult(null));
            }

            [Fact]
            public void GivenValidMessage_ExecuteResults_WritesMessageToContext()
            {
                var sut = new ErrorResult(Message, Mapper);

                var context = new ActionContext()
                {
                    HttpContext = Substitute.For<HttpContext>()
                };

                sut.ExecuteResult(context);
            }
        }

        public class Fixture
        {
            public string Message { get; }

            public IMapper Mapper { get; }

            public Fixture() 
            {
                Message = "a message";
                if (Mapper == null)
                {
                    var mappingConfig = new MapperConfiguration(mc =>
                    {
                        mc.CreateMap<IErrorMessage, ErrorMessageDto>();
                    });
                    IMapper mapper = mappingConfig.CreateMapper();
                    Mapper = mapper;
                }

            }

        }
    }
}
