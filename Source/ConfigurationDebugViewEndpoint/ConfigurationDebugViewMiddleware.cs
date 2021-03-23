namespace ConfigurationDebugViewEndpoint
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Termination middleware that exposes configuration-debug-view endpoint
    /// </summary>
    public class ConfigurationDebugViewMiddleware
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ConfigurationDebugViewMiddleware"/>.
        /// </summary>
        /// <param name="next"></param>
        public ConfigurationDebugViewMiddleware(RequestDelegate next) { }

        /// <summary>
        /// Executes the middleware that provides configuration-debug-view page.
        /// </summary>
        /// <param name="httpContext">The <see cref="HttpContext"/> for the current request.</param>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var configuration = httpContext.RequestServices.GetService<IConfiguration>();
            var config = (configuration as IConfigurationRoot).GetDebugView();
            await httpContext.Response.WriteAsync(config);
        }
    }
}
