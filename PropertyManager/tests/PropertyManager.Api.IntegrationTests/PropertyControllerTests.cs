namespace PropertyManager.Api.IntegrationTests
{
    using Newtonsoft.Json;
    using PropertyManager.Api.Dto;
    using PropertyManager.Api.ErrorResults;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;

    public class PropertyControllerTests
    {
        public class Get : Fixture
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-2)]
            public async Task GivenInvalidRequest_Get_ReturnsBadRequest(int id)
            {
                var response = await Client.GetAsync($"/Property/{id}");


                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            [Fact]
            public async Task GivenValidIdAndDoesNotExist_Get_ReturnsNoContent()
            {
                var response = await Client.GetAsync("/Property/999");
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }

            [Fact]
            public async Task GivenValidIdAndExists_Get_ReturnsContent()
            {
                var response = await Client.GetAsync("/Property/10");
                var property = await response.Content.ReadFromJsonAsync<PropertyDto>();

                Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(property);
                Assert.Equal(10, property.Id);
                Assert.Equal("Apartment", property.Type);
                Assert.Equal("line 1", property.AddressLine1);
                Assert.Equal("line 2", property.AddressLine2);
                Assert.Equal("city", property.City);
                Assert.Equal("region", property.Region);
                Assert.Equal("postcode", property.PostalCode);
                Assert.Equal(100000, property.PurchasePrice.Value);
                Assert.Equal(new DateOnly(2023, 02, 20), property.PurchaseDate.Value);
                Assert.True(property.Garage);
                Assert.Equal(3, property.NumberOfParkingSpaces.Value);
                Assert.Equal("some notes", property.Notes);
            }
        }

        public class Delete : Fixture
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-2)]
            public async Task GivenInvalidRequest_Delete_ReturnsBadRequest(int id)
            {
                var response = await Client.DeleteAsync($"/Property/{id}");
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            [Fact]
            public async Task GivenValidIdAndDoesNotExist_Delete_ReturnsNotFound()
            {
                var response = await Client.DeleteAsync("/Property/999");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            [Fact]
            public async Task GivenValidIdAndExists_Delete_ReturnsContentContent()
            {
                var response = await Client.DeleteAsync("/Property/10");
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

                var response = await Client.PostAsync("/Property/", content);
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            [Fact]
            public async Task GivenIdProvided_Post_ReturnsBadRequest()
            {
                var property = new PropertyDto { Id = 4, Type = "test", AddressLine1 = "", AddressLine2 = "", City = "", Region = "", PostalCode = "", Notes = "" };

                var content = new StringContent(JsonConvert.SerializeObject(property), MediaTypeHeaderValue.Parse("application/json"));
                var response = await Client.PostAsync("/Property/", content);
                var body = await response.Content.ReadAsStringAsync();
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.Equal("Id cannot be provided when creating a property.", body);
            }

            [Fact]
            public async Task GivenValidRequestFailed_Post_ReturnsErrorMessage()
            {
                var property = new PropertyDto { Type = "test", AddressLine1 = "line1", AddressLine2 = "", City = "city", Region = "", PostalCode = "postcode", Notes = "fail" };

                var payload = new StringContent(JsonConvert.SerializeObject(property), MediaTypeHeaderValue.Parse("application/json"));
                var response = await Client.PostAsync("/Property/", payload);
                var content = await response.Content.ReadFromJsonAsync<ErrorMessageDto>();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
                Assert.NotNull(content);
                Assert.Equal("The property could not be created.", content.Message);

            }
            [Fact]
            public async Task GivenValidRequest_Post_ReturnsCreated()
            {
                var property = new PropertyDto { Type = "A property type", AddressLine1 = "line1", AddressLine2 = "line2", City = "city", Region = "region", PostalCode = "postcode", Notes = "some notes", Garage = true, NumberOfParkingSpaces = 3, PurchaseDate = new DateOnly(2023,02,21), PurchasePrice = 10000000 };

                var payload = new StringContent(JsonConvert.SerializeObject(property), Encoding.UTF8, "application/json");

                var response = await Client.PostAsync("/Property/", payload);
                var content = await response.Content.ReadFromJsonAsync<PropertyDto>();
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
                Assert.NotNull(content);
                Assert.Equal(10, content.Id);
                Assert.Equal("A property type", content.Type);
                Assert.Equal("line1", content.AddressLine1);
                Assert.Equal("line2", content.AddressLine2);
                Assert.Equal("city", content.City);
                Assert.Equal("region", content.Region);
                Assert.Equal("postcode", content.PostalCode);
                Assert.Equal("some notes", content.Notes);
                Assert.True(content.Garage);
                Assert.Equal(3, content.NumberOfParkingSpaces.Value);
                Assert.Equal(new DateOnly(2023, 02, 21), content.PurchaseDate);
                Assert.Equal(10000000, content.PurchasePrice.Value);
            }
        }

        public class Update : Fixture
        {
            [Fact]
            public async Task GivenEmptyJson_Put_ReturnsBadRequest()
            {
                var content = new StringContent(string.Empty, MediaTypeHeaderValue.Parse("application/json"));

                var response = await Client.PutAsync("/Property/", content);
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            [Fact]
            public async Task GivenIdNotProvided_Put_ReturnsBadRequest()
            {
                var property = new PropertyDto { Type = "test", AddressLine1 = "", AddressLine2 = "", City = "", Region = "", PostalCode = "", Notes = "" };

                var content = new StringContent(JsonConvert.SerializeObject(property), MediaTypeHeaderValue.Parse("application/json"));
                var response = await Client.PutAsync("/Property/", content);
                var body = await response.Content.ReadAsStringAsync();
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.Equal("Id must be provided when updating a property.", body);
            }

            [Fact]
            public async Task GivenValidRequestFailed_Post_ReturnsErrorMessage()
            {
                var property = new PropertyDto { Id = 123, Type = "test", AddressLine1 = "line1", AddressLine2 = "", City = "city", Region = "", PostalCode = "postcode", Notes = "fail" };

                var payload = new StringContent(JsonConvert.SerializeObject(property), MediaTypeHeaderValue.Parse("application/json"));
                var response = await Client.PutAsync("/Property/", payload);
                var content = await response.Content.ReadFromJsonAsync<ErrorMessageDto>();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
                Assert.NotNull(content);
                Assert.Equal("The property could not be updated.", content.Message);

            }
            [Fact]
            public async Task GivenValidRequest_Post_ReturnsUpdated()
            {
                var property = new PropertyDto { Id = 10, Type = "A property type", AddressLine1 = "line1", AddressLine2 = "line2", City = "city", Region = "region", PostalCode = "postcode", Notes = "some notes", Garage = true, NumberOfParkingSpaces = 3, PurchaseDate = new DateOnly(2023, 02, 21), PurchasePrice = 10000000 };

                var payload = new StringContent(JsonConvert.SerializeObject(property), MediaTypeHeaderValue.Parse("application/json"));
                var response = await Client.PutAsync("/Property/", payload);
                var content = await response.Content.ReadFromJsonAsync<PropertyDto>();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
                Assert.NotNull(content);
                Assert.Equal(10, content.Id);
                Assert.Equal("A property type", content.Type);
                Assert.Equal("line1", content.AddressLine1);
                Assert.Equal("line2", content.AddressLine2);
                Assert.Equal("city", content.City);
                Assert.Equal("region", content.Region);
                Assert.Equal("postcode", content.PostalCode);
                Assert.Equal("some notes", content.Notes);
                Assert.True(content.Garage);
                Assert.Equal(3, content.NumberOfParkingSpaces.Value);
                Assert.Equal(new DateOnly(2023, 02, 21), content.PurchaseDate);
                Assert.Equal(10000000, content.PurchasePrice.Value);
            }
        }
    }
}
