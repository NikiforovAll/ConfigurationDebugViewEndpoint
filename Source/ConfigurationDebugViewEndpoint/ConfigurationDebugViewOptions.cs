namespace ConfigurationDebugViewEndpoint
{
    /// <summary>
    /// Contains options for the <see cref="ConfigurationDebugViewMiddleware"/>.
    /// </summary>
    public class ConfigurationDebugViewOptions
    {
        /// <summary>
        /// Gets or sets a value that controls whether to use <see cref="ConfigurationDebugViewMiddleware"/>
        /// exclusively on Development environment.
        /// </summary>
        public bool AllowDevelopmentOnly { get; set; } = true;
    }
}
