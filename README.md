# NikiforovAll.ConfigurationDebugViewEndpoint

[![GitHub Actions Status](https://github.com/NikiforovAll/ConfigurationDebugViewEndpoint/workflows/Build/badge.svg?branch=main)](https://github.com/NikiforovAll/ConfigurationDebugViewEndpoint/actions)

[![GitHub Actions Build History](https://buildstats.info/github/chart/nikiforovall/ConfigurationDebugViewEndpoint?branch=main&includeBuildsFromPullRequest=false)](https://github.com/NikiforovAll/ConfigurationDebugViewEndpoint/actions)

A convenient extension method(s) for IEndpointRouteBuilder to add configuration debug view.

The goal of this project is to provide a convenient way of adding something like this to a project:

Before:

```csharp
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/config", async context =>
    {
        var config = (Configuration as IConfigurationRoot).GetDebugView();
        await context.Response.WriteAsync(config);
    });
});
```

After:

```csharp
app.UseEndpoints(endpoints =>
{
    endpoints.MapConfigurationDebugView("/config", (options) => options.AllowDevelopmentOnly = true);
});
```

For more examples, please see: [tests](.\Tests\ConfigurationDebugViewEndpoint.Test\EndpointRouteBuilderExtensionsTests.cs)
