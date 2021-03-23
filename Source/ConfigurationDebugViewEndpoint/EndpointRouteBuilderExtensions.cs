namespace ConfigurationDebugViewEndpoint.Extensions
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;

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
        /// <returns>A route for the endpoint.</returns>
        public static IEndpointConventionBuilder MapConfigurationDebugView(
            this IEndpointRouteBuilder endpoints,
            string pattern = "config")
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            return MapConfigurationDebugViewCore(endpoints, pattern);
        }

        private static IEndpointConventionBuilder MapConfigurationDebugViewCore(
            IEndpointRouteBuilder endpoints,
            string pattern)
        {
            var pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<ConfigurationDebugViewMiddleware>()
                .Build();

            return endpoints.Map(pattern, pipeline);
        }
    }
}
