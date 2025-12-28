using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Trix;

/// <summary>
/// Configuration options for the Trix client.
/// </summary>
public class TrixClientOptions
{
    /// <summary>
    /// The default base URL for the Trix API.
    /// </summary>
    public const string DefaultBaseUrl = "https://api.trixdb.com";

    /// <summary>
    /// The default timeout for requests.
    /// </summary>
    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    /// <summary>
    /// The maximum allowed timeout (1 hour).
    /// </summary>
    public static readonly TimeSpan MaxTimeout = TimeSpan.FromHours(1);

    /// <summary>
    /// The default maximum number of retries.
    /// </summary>
    public const int DefaultMaxRetries = 3;

    /// <summary>
    /// The maximum allowed number of retries.
    /// </summary>
    public const int MaxAllowedRetries = 10;

    /// <summary>
    /// HTTP headers that cannot be overridden via CustomHeaders for security reasons.
    /// </summary>
    private static readonly HashSet<string> RestrictedHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Authorization",
        "Host",
        "Content-Length",
        "Transfer-Encoding",
        "Connection",
        "Keep-Alive",
        "Proxy-Authorization",
        "Proxy-Connection",
        "TE",
        "Trailer",
        "Upgrade"
    };

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
    /// Gets or sets the base URL for the Trix API.
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
    /// Reads TRIX_API_KEY and optionally TRIX_BASE_URL.
    /// </summary>
    /// <returns>Configured options.</returns>
    /// <exception cref="InvalidOperationException">If TRIX_API_KEY is not set.</exception>
    public static TrixClientOptions FromEnvironment()
    {
        var apiKey = Environment.GetEnvironmentVariable("TRIX_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException(
                "TRIX_API_KEY environment variable is not set.");
        }

        var options = new TrixClientOptions { ApiKey = apiKey };

        var baseUrl = Environment.GetEnvironmentVariable("TRIX_BASE_URL");
        if (!string.IsNullOrEmpty(baseUrl))
        {
            options.BaseUrl = baseUrl;
        }

        return options;
    }

    /// <summary>
    /// Validates the options.
    /// </summary>
    /// <exception cref="ArgumentException">If required options are missing or invalid.</exception>
    public void Validate()
    {
        if (string.IsNullOrEmpty(ApiKey) && string.IsNullOrEmpty(JwtToken))
        {
            throw new ArgumentException(
                "Either ApiKey or JwtToken must be provided.");
        }

        // Validate BaseUrl is a valid absolute URI
        if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out var uri) ||
            (uri.Scheme != "http" && uri.Scheme != "https"))
        {
            throw new ArgumentException(
                "BaseUrl must be a valid absolute URI with http or https scheme.");
        }

        // Validate BaseUrl uses HTTPS (unless AllowInsecure is true)
        if (!AllowInsecure && !uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "BaseUrl must use HTTPS. Set AllowInsecure to true for development.");
        }

        // Validate timeout is positive and within bounds
        if (Timeout <= TimeSpan.Zero)
        {
            throw new ArgumentException("Timeout must be positive.");
        }

        if (Timeout > MaxTimeout)
        {
            throw new ArgumentException(
                $"Timeout cannot exceed {MaxTimeout.TotalHours} hour(s).");
        }

        // Validate MaxRetries is within bounds
        if (MaxRetries < 0)
        {
            throw new ArgumentException("MaxRetries cannot be negative.");
        }

        if (MaxRetries > MaxAllowedRetries)
        {
            throw new ArgumentException(
                $"MaxRetries cannot exceed {MaxAllowedRetries}.");
        }

        // Validate custom headers don't contain restricted headers
        ValidateCustomHeaders();
    }

    /// <summary>
    /// Validates that custom headers don't contain restricted header names.
    /// </summary>
    private void ValidateCustomHeaders()
    {
        if (CustomHeaders == null || CustomHeaders.Count == 0)
        {
            return;
        }

        foreach (var headerName in CustomHeaders.Keys)
        {
            if (RestrictedHeaders.Contains(headerName))
            {
                throw new ArgumentException(
                    $"Cannot set restricted header '{headerName}' via CustomHeaders. " +
                    "This header is managed by the SDK for security reasons.");
            }
        }
    }
}
