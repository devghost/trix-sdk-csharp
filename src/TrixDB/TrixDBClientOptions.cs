using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace TrixDB;

/// <summary>
/// Configuration options for the TrixDB client.
/// </summary>
public class TrixDBClientOptions
{
    /// <summary>
    /// The default base URL for the TrixDB API.
    /// </summary>
    public const string DefaultBaseUrl = "https://api.trixdb.com";

    /// <summary>
    /// The default timeout for requests.
    /// </summary>
    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    /// <summary>
    /// The default maximum number of retries.
    /// </summary>
    public const int DefaultMaxRetries = 3;

    /// <summary>
    /// Gets or sets the API key for authentication.
    /// Either ApiKey or JwtToken must be provided.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Gets or sets the JWT token for authentication.
    /// Either ApiKey or JwtToken must be provided.
    /// </summary>
    public string? JwtToken { get; set; }

    /// <summary>
    /// Gets or sets the base URL for the TrixDB API.
    /// Default: https://api.trixdb.com
    /// </summary>
    public string BaseUrl { get; set; } = DefaultBaseUrl;

    /// <summary>
    /// Gets or sets the request timeout.
    /// Default: 30 seconds.
    /// </summary>
    public TimeSpan Timeout { get; set; } = DefaultTimeout;

    /// <summary>
    /// Gets or sets the maximum number of retry attempts.
    /// Default: 3.
    /// </summary>
    public int MaxRetries { get; set; } = DefaultMaxRetries;

    /// <summary>
    /// Gets or sets whether to allow insecure connections (HTTP).
    /// Only use for local development. Default: false.
    /// </summary>
    public bool AllowInsecure { get; set; } = false;

    /// <summary>
    /// Gets or sets the logger factory for SDK logging.
    /// </summary>
    public ILoggerFactory? LoggerFactory { get; set; }

    /// <summary>
    /// Gets or sets custom headers to include in all requests.
    /// </summary>
    public Dictionary<string, string>? CustomHeaders { get; set; }

    /// <summary>
    /// Gets or sets the HTTP client handler for custom configuration.
    /// </summary>
    public HttpMessageHandler? HttpHandler { get; set; }

    /// <summary>
    /// Creates options from environment variables.
    /// Reads TRIXDB_API_KEY and optionally TRIXDB_BASE_URL.
    /// </summary>
    /// <returns>Configured options.</returns>
    /// <exception cref="InvalidOperationException">If TRIXDB_API_KEY is not set.</exception>
    public static TrixDBClientOptions FromEnvironment()
    {
        var apiKey = Environment.GetEnvironmentVariable("TRIXDB_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException(
                "TRIXDB_API_KEY environment variable is not set.");
        }

        var options = new TrixDBClientOptions { ApiKey = apiKey };

        var baseUrl = Environment.GetEnvironmentVariable("TRIXDB_BASE_URL");
        if (!string.IsNullOrEmpty(baseUrl))
        {
            options.BaseUrl = baseUrl;
        }

        return options;
    }

    /// <summary>
    /// Validates the options.
    /// </summary>
    /// <exception cref="ArgumentException">If required options are missing.</exception>
    public void Validate()
    {
        if (string.IsNullOrEmpty(ApiKey) && string.IsNullOrEmpty(JwtToken))
        {
            throw new ArgumentException(
                "Either ApiKey or JwtToken must be provided.");
        }

        if (!AllowInsecure && !BaseUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "BaseUrl must use HTTPS. Set AllowInsecure to true for development.");
        }

        if (Timeout <= TimeSpan.Zero)
        {
            throw new ArgumentException("Timeout must be positive.");
        }

        if (MaxRetries < 0)
        {
            throw new ArgumentException("MaxRetries cannot be negative.");
        }
    }
}
