namespace PropertyManager.Api.IntegrationTests
{
    using Newtonsoft.Json;
    using PropertyManager.Api.Dto;
    using PropertyManager.Api.ErrorResults;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class PropertyTypeControllerTests
    {
        public class Get : Fixture
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-2)]
            public async Task GivenInvalidRequest_Get_ReturnsBadRequest(int id)
            {
                var response = await Client.GetAsync($"/PropertyType/{id}");


                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            [Fact]
            public async Task GivenValidIdAndDoesNotExist_Get_ReturnsNoContent()
            {
                var response = await Client.GetAsync("/PropertyType/999");
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }

            [Fact]
            public async Task GivenValidIdAndExists_Get_ReturnsContent()
            {
                var response = await Client.GetAsync("/PropertyType/10");
                var content = await response.Content.ReadFromJsonAsync<PropertyTypeDto>();

                Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(content);
                Assert.Equal(10, content.Id);
                Assert.Equal("A property type", content.Name);
            }
        }       

        public class Delete : Fixture
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-2)]
            public async Task GivenInvalidRequest_Delete_ReturnsBadRequest(int id)
            {
                var response = await Client.DeleteAsync($"/PropertyType/{id}");
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            [Fact]
            public async Task GivenValidIdAndDoesNotExist_Delete_ReturnsNotFound()
            {
                var response = await Client.DeleteAsync("/PropertyType/999");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            [Fact]
            public async Task GivenValidIdAndExists_Delete_ReturnsContentContent()
            {
                var response = await Client.DeleteAsync("/PropertyType/10");
                var content = await response.Content.ReadAsStringAsync();

                Assert.Equal(string.Empty, content);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        public class Save : Fixture
        {
            [Fact]
            public async Task GivenEmptyJson_Post_ReturnsBadRequest()
            {
                var content = new StringContent(string.Empty, MediaTypeHeaderValue.Parse("application/json"));

                var response = await Client.PostAsync("/PropertyType/", content);
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            [Fact]
            public async Task GivenIdProvided_Post_ReturnsBadRequest()
            {
                var propertyType = new PropertyTypeDto { Id = 4, Name = "test" };

                var content = new StringContent(JsonConvert.SerializeObject(propertyType), MediaTypeHeaderValue.Parse("application/json"));
                var response = await Client.PostAsync("/PropertyType/", content);
                var body = await response.Content.ReadAsStringAsync();
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.Equal("Id cannot be provided when creating a property type.", body);
            }

            [Fact]
            public async Task GivenValidRequestFailed_Post_ReturnsErrorMessage()
            {
                var propertyType = new PropertyTypeDto { Name = "fail" };

                var payload = new StringContent(JsonConvert.SerializeObject(propertyType), MediaTypeHeaderValue.Parse("application/json"));
                var response = await Client.PostAsync("/PropertyType/", payload);
                var content = await response.Content.ReadFromJsonAsync<ErrorMessageDto>();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
                Assert.NotNull(content);
                Assert.Equal("The property type could not be created.", content.Message);

            }
            [Fact]
            public async Task GivenValidRequest_Post_ReturnsCreated()
            {
                var propertyType = new PropertyTypeDto { Name = "test" };

                var payload = new StringContent(JsonConvert.SerializeObject(propertyType), Encoding.UTF8, "application/json");
                
                var response = await Client.PostAsync("/PropertyType/", payload);
                var content = await response.Content.ReadFromJsonAsync<PropertyTypeDto>();
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
                Assert.NotNull(content);
                Assert.Equal(999, content.Id);
                Assert.Equal("test", content.Name);
            }
        }

        public class Update : Fixture
        {
            [Fact]
            public async Task GivenEmptyJson_Put_ReturnsBadRequest()
            {
                var content = new StringContent(string.Empty, MediaTypeHeaderValue.Parse("application/json"));

                var response = await Client.PutAsync("/PropertyType/", content);
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            [Fact]
            public async Task GivenIdNotProvided_Put_ReturnsBadRequest()
            {
                var propertyType = new PropertyTypeDto { Name = "test" };

                var content = new StringContent(JsonConvert.SerializeObject(propertyType), MediaTypeHeaderValue.Parse("application/json"));
                var response = await Client.PutAsync("/PropertyType/", content);
                var body = await response.Content.ReadAsStringAsync();
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.Equal("Id must be provided when updating a property type.", body);
            }

            [Fact]
            public async Task GivenValidRequestFailed_Post_ReturnsErrorMessage()
            {
                var propertyType = new PropertyTypeDto { Id = 123, Name = "fail" };

                var payload = new StringContent(JsonConvert.SerializeObject(propertyType), MediaTypeHeaderValue.Parse("application/json"));
                var response = await Client.PutAsync("/PropertyType/", payload);
                var content = await response.Content.ReadFromJsonAsync<ErrorMessageDto>();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
                Assert.NotNull(content);
                Assert.Equal("The property type could not be updated.", content.Message);

            }
            [Fact]
            public async Task GivenValidRequest_Post_ReturnsCreated()
            {
                var propertyType = new PropertyTypeDto { Id = 999, Name = "test" };

                var payload = new StringContent(JsonConvert.SerializeObject(propertyType), MediaTypeHeaderValue.Parse("application/json"));
                var response = await Client.PutAsync("/PropertyType/", payload);
                var content = await response.Content.ReadFromJsonAsync<PropertyTypeDto>();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
                Assert.NotNull(content);
                Assert.Equal(999, content.Id);
                Assert.Equal("test", content.Name);
            }
        }
    }
}
