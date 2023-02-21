namespace PropertyManager.Api.IntegrationTests
{
    /// <summary>
    /// A fixture to initialise shared startup configuration for all tests
    /// </summary>
    public class Fixture : CustomWebApplicationFactory
    { 
        /// <summary>
        /// Initalises a new instance of the <see cref="Fixture"/> class.
        /// </summary>

        public Fixture()
        {
            Client = CreateClient();

        }

        /// <summary>
        /// Gets the client used to send requests to the test server
        /// </summary>
        public HttpClient Client { get; }
    }
}
