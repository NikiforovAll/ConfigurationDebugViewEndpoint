namespace ConfigurationDebugViewEndpoint.Extensions
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Provides extension methods for <see cref="IEndpointRouteBuilder"/> to add routes.
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Adds a configuration endpoint to the <see cref="IEndpointRouteBuilder"/> with the specified template.
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> to add endpoint to.</param>
        /// <param name="pattern">The URL pattern of the endpoint.</param>
        /// <param name="optionsDelegate"></param>
        /// <returns>A route for the endpoint.</returns>
        public static IEndpointConventionBuilder? MapConfigurationDebugView(
            this IEndpointRouteBuilder endpoints,
            string pattern = "config",
            Action<ConfigurationDebugViewOptions>? optionsDelegate = default)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            var options = new ConfigurationDebugViewOptions();
            optionsDelegate?.Invoke(options);

            return MapConfigurationDebugViewCore(endpoints, pattern, options);
        }

        private static IEndpointConventionBuilder? MapConfigurationDebugViewCore(
            IEndpointRouteBuilder endpoints,
            string pattern, ConfigurationDebugViewOptions options)
        {
            var environment = endpoints.ServiceProvider.GetRequiredService<IHostEnvironment>();
            var builder = endpoints.CreateApplicationBuilder();

            if (options.AllowDevelopmentOnly && !environment.IsDevelopment())
            {
                return null;
            }
            var pipeline = builder
                .UseMiddleware<ConfigurationDebugViewMiddleware>()
                .Build();

            return endpoints.Map(pattern, pipeline);
        }
    }
}
