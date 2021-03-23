namespace ConfigurationDebugViewEndpoint.Test
{
    using System.Threading.Tasks;
    using ConfigurationDebugViewEndpoint;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Hosting;
    using Xunit;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using System;
    using ConfigurationDebugViewEndpoint.Extensions;

    public class EndpointRouteBuilderExtensionsTests
    {
        [Fact]
        public async Task EmptyConfiguration()
        {
            using var host = new HostBuilder().ConfigureWebHost(builder =>
                {
                    builder.Configure(app => app.UseMiddleware<ConfigurationDebugViewMiddleware>());
                    builder.UseTestServer();
                })
                .Build();

            await host.StartAsync();
            using var server = host.GetTestServer();
            var response = await server.CreateClient().GetAsync("http://example.com/config");
            Assert.Equal(200, (int)response.StatusCode);
            Assert.NotEmpty(await response.Content.ReadAsStringAsync());
        }


        [Fact]
        public async Task NotEmptyConfiguration_ExpectedConfigurationReturned()
        {
            var settingKey = "UniqueConfigurationValueNotExpectedToBeProvidedByDotnetFramework";
            var settingValue = Guid.NewGuid().ToString();
            using var host = new HostBuilder().ConfigureWebHost(builder =>
                {
                    builder.Configure(app => app.UseMiddleware<ConfigurationDebugViewMiddleware>());
                    builder.UseTestServer()
                        .UseSetting("UniqueConfigurationValueNotExpectedToBeProvidedByDotnetFramework", settingValue);
                })
                .Build();

            await host.StartAsync();
            using var server = host.GetTestServer();
            var response = await server.CreateClient().GetAsync("http://example.com/config");
            Assert.Equal(200, (int)response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains(settingKey, content);
            Assert.Contains(settingValue, content);
        }

        [Theory]
        [InlineData("Development", 200)]
        [InlineData("Production", 404)]
        public async Task WithAllowDevelopmentOnlyOptions_Development_ConfigurationReturned(
            string environment,
            int expectedStatusCode)
        {
            var pattern = "/config";
            using var host = new HostBuilder().ConfigureWebHost(builder =>
            {
                builder.ConfigureServices(services => services.AddRouting());
                builder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                        endpoints.MapConfigurationDebugView(pattern, (options) => options.AllowDevelopmentOnly = true));
                });
                builder.UseTestServer()
                    .UseEnvironment(environment);
            })
                .Build();

            await host.StartAsync();
            using var server = host.GetTestServer();
            var response = await server.CreateClient().GetAsync($"http://example.com{pattern}");
            Assert.Equal(expectedStatusCode, (int)response.StatusCode);
        }

    }
}
