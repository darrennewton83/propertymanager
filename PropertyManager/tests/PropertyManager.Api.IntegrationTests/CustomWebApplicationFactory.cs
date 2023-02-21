namespace PropertyManager.Api.IntegrationTests
{
    using PropertyManager.Api.IntegrationTests.MockDataStores;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using NSubstitute;
    using PropertyManager.Shared.Property.DataStores;
    using PropertyManager.Shared.PropertyType.DataStores;

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
                services.AddTransient<IPropertyDataStore, MockPropertyDataStore>();
            });
        }
    }
}
