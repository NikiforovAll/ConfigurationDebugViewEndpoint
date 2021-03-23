namespace WebEmpty
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class WithoutRouteBuilderStartup
    {
        public IConfiguration Configuration { get; set; }

        public WithoutRouteBuilderStartup(IConfiguration configuration) => this.Configuration = configuration;

        public void ConfigureServices(IServiceCollection services) { }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/config", async context =>
                {
                    var config = (Configuration as IConfigurationRoot).GetDebugView();
                    await context.Response.WriteAsync(config);
                });
            });
        }
    }
}
