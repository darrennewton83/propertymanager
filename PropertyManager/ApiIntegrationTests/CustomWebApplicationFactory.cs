namespace ApiIntegrationTests
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using NSubstitute;
    using Service.propertyType.DataStores;

    /// <summary>
    /// A web application factory to create a test server
    /// </summary>
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        /// <inheritdoc />
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            var propertyTypeDataStore = Substitute.For<IPropertyTypeDataStore>();
            builder.UseTestServer();
            builder.ConfigureServices(services =>
            {
                services.AddTransient<IPropertyTypeDataStore, MockPropertyTypeDataStore>();
            });
        }
    }
}
