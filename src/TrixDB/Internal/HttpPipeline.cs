using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TrixDB.Exceptions;

namespace TrixDB.Internal;

/// <summary>
/// HTTP pipeline for making API requests with retry and error handling.
/// </summary>
internal sealed class HttpPipeline : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TrixDBClientOptions _options;
    private readonly ILogger _logger;
    private readonly bool _ownsHttpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// SDK version for user agent.
    /// </summary>
    public const string SdkVersion = "1.0.0";

    /// <summary>
    /// API version.
    /// </summary>
    public const string ApiVersion = "v1";

    public HttpPipeline(TrixDBClientOptions options, HttpClient? httpClient = null)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _options.Validate();

        var loggerFactory = options.LoggerFactory ?? NullLoggerFactory.Instance;
        _logger = loggerFactory.CreateLogger<HttpPipeline>();

        if (httpClient != null)
        {
            _httpClient = httpClient;
            _ownsHttpClient = false;
        }
        else
        {
            _httpClient = CreateHttpClient(options);
            _ownsHttpClient = true;
        }
    }

    private static HttpClient CreateHttpClient(TrixDBClientOptions options)
    {
        var handler = options.HttpHandler ?? new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(10),
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
            MaxConnectionsPerServer = 20
        };

        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri(options.BaseUrl),
            Timeout = options.Timeout
        };

        // Set default headers
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.UserAgent.ParseAdd($"trixdb-csharp-sdk/{SdkVersion}");
        client.DefaultRequestHeaders.Add("X-SDK-Version", SdkVersion);
        client.DefaultRequestHeaders.Add("X-API-Version", ApiVersion);

        // Set authentication
        if (!string.IsNullOrEmpty(options.ApiKey))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.ApiKey);
        }
        else if (!string.IsNullOrEmpty(options.JwtToken))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.JwtToken);
        }

        // Add custom headers
        if (options.CustomHeaders != null)
        {
            foreach (var (key, value) in options.CustomHeaders)
            {
                client.DefaultRequestHeaders.Add(key, value);
            }
        }

        return client;
    }

    /// <summary>
    /// Sends an HTTP request with retry logic.
    /// </summary>
    public async Task<HttpResponseMessage> SendAsync(
        HttpMethod method,
        string path,
        object? body = null,
        Dictionary<string, string?>? queryParams = null,
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(path, queryParams);
        var attempt = 0;
        Exception? lastException = null;

        while (attempt <= _options.MaxRetries)
        {
            try
            {
                using var request = new HttpRequestMessage(method, url);

                if (body != null)
                {
                    request.Content = JsonContent.Create(body, options: JsonOptions);
                }

                _logger.LogDebug("Request: {Method} {Url} (attempt {Attempt})", method, url, attempt + 1);

                var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

                _logger.LogDebug("Response: {StatusCode} from {Url}", (int)response.StatusCode, url);

                await HandleResponseAsync(response, cancellationToken).ConfigureAwait(false);

                return response;
            }
            catch (HttpRequestException ex)
            {
                lastException = new NetworkException($"Network error: {ex.Message}", ex);
                _logger.LogWarning(ex, "Network error on attempt {Attempt}", attempt + 1);
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                lastException = new TrixDBTimeoutException($"Request timed out after {_options.Timeout}", ex, _options.Timeout);
                _logger.LogWarning("Request timed out on attempt {Attempt}", attempt + 1);
            }
            catch (RateLimitException ex)
            {
                lastException = ex;
                if (attempt < _options.MaxRetries && ex.RetryAfterSeconds.HasValue)
                {
                    var delay = TimeSpan.FromSeconds(ex.RetryAfterSeconds.Value);
                    _logger.LogWarning("Rate limited. Waiting {Seconds}s before retry", delay.TotalSeconds);
                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                }
            }
            catch (ServerException ex)
            {
                lastException = ex;
                _logger.LogWarning(ex, "Server error on attempt {Attempt}", attempt + 1);
            }
            catch (TrixDBException)
            {
                throw; // Don't retry client errors
            }

            attempt++;

            if (attempt <= _options.MaxRetries)
            {
                var delay = CalculateBackoff(attempt);
                _logger.LogDebug("Retrying in {Delay}ms", delay.TotalMilliseconds);
                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
            }
        }

        throw lastException ?? new TrixDBException("Request failed after all retries");
    }

    /// <summary>
    /// Sends a multipart form data request for file uploads.
    /// </summary>
    public async Task<HttpResponseMessage> SendMultipartAsync(
        string path,
        Stream fileData,
        string fileName,
        string contentType,
        Dictionary<string, object>? additionalFields = null,
        CancellationToken cancellationToken = default)
    {
        var attempt = 0;
        Exception? lastException = null;

        while (attempt <= _options.MaxRetries)
        {
            try
            {
                using var content = new MultipartFormDataContent();

                // Add file content
                var streamContent = new StreamContent(fileData);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                content.Add(streamContent, "file", fileName);

                // Add additional form fields
                if (additionalFields != null)
                {
                    foreach (var (key, value) in additionalFields)
                    {
                        if (value != null)
                        {
                            var jsonValue = JsonSerializer.Serialize(value, JsonOptions);
                            content.Add(new StringContent(jsonValue), key);
                        }
                    }
                }

                using var request = new HttpRequestMessage(HttpMethod.Post, path)
                {
                    Content = content
                };

                _logger.LogDebug("Multipart Request: POST {Path} (attempt {Attempt})", path, attempt + 1);

                var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

                _logger.LogDebug("Response: {StatusCode} from {Path}", (int)response.StatusCode, path);

                await HandleResponseAsync(response, cancellationToken).ConfigureAwait(false);

                return response;
            }
            catch (HttpRequestException ex)
            {
                lastException = new NetworkException($"Network error: {ex.Message}", ex);
                _logger.LogWarning(ex, "Network error on attempt {Attempt}", attempt + 1);
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                lastException = new TrixDBTimeoutException($"Request timed out after {_options.Timeout}", ex, _options.Timeout);
                _logger.LogWarning("Request timed out on attempt {Attempt}", attempt + 1);
            }
            catch (TrixDBException)
            {
                throw; // Don't retry client errors
            }

            attempt++;

            if (attempt <= _options.MaxRetries)
            {
                // Reset stream position for retry
                if (fileData.CanSeek)
                {
                    fileData.Position = 0;
                }

                var delay = CalculateBackoff(attempt);
                _logger.LogDebug("Retrying in {Delay}ms", delay.TotalMilliseconds);
                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
            }
        }

        throw lastException ?? new TrixDBException("Request failed after all retries");
    }

    private async Task HandleResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var errorBody = await ReadErrorBodyAsync(response, cancellationToken).ConfigureAwait(false);
        var message = errorBody?.Message ?? response.ReasonPhrase ?? "Unknown error";
        var errorCode = errorBody?.Code;
        var requestId = response.Headers.TryGetValues("X-Request-Id", out var values)
            ? values.FirstOrDefault()
            : null;

        throw response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => new AuthenticationException(message, errorCode, requestId),
            HttpStatusCode.Forbidden => new PermissionException(message, errorCode, requestId),
            HttpStatusCode.NotFound => new NotFoundException(message, errorCode: errorCode, requestId: requestId),
            HttpStatusCode.UnprocessableEntity => new ValidationException(message, errorCode: errorCode, requestId: requestId),
            HttpStatusCode.TooManyRequests => CreateRateLimitException(response, message, errorCode, requestId),
            >= HttpStatusCode.InternalServerError => new ServerException(message, response.StatusCode, errorCode, requestId),
            _ => new TrixDBException(message, response.StatusCode, errorCode, requestId)
        };
    }

    private static RateLimitException CreateRateLimitException(
        HttpResponseMessage response,
        string message,
        string? errorCode,
        string? requestId)
    {
        int? retryAfter = null;
        DateTimeOffset? resetAt = null;

        if (response.Headers.TryGetValues("Retry-After", out var retryValues))
        {
            var retryValue = retryValues.FirstOrDefault();
            if (int.TryParse(retryValue, out var seconds))
            {
                retryAfter = seconds;
            }
            else if (DateTimeOffset.TryParse(retryValue, out var date))
            {
                resetAt = date;
                retryAfter = (int)(date - DateTimeOffset.UtcNow).TotalSeconds;
            }
        }

        return new RateLimitException(message, retryAfter, resetAt, errorCode, requestId);
    }

    private static async Task<ErrorResponse?> ReadErrorBodyAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        try
        {
            return await response.Content.ReadFromJsonAsync<ErrorResponse>(JsonOptions, cancellationToken)
                .ConfigureAwait(false);
        }
        catch
        {
            return null;
        }
    }

    private static string BuildUrl(string path, Dictionary<string, string?>? queryParams)
    {
        if (queryParams == null || queryParams.Count == 0)
        {
            return path;
        }

        var queryString = string.Join("&",
            queryParams
                .Where(kvp => kvp.Value != null)
                .Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value!)}"));

        return string.IsNullOrEmpty(queryString) ? path : $"{path}?{queryString}";
    }

    private static TimeSpan CalculateBackoff(int attempt)
    {
        var baseDelay = 1000; // 1 second
        var maxDelay = 30000; // 30 seconds
        var delay = Math.Min(baseDelay * Math.Pow(2, attempt - 1), maxDelay);
        var jitter = Random.Shared.NextDouble() * 0.3 * delay; // 0-30% jitter
        return TimeSpan.FromMilliseconds(delay + jitter);
    }

    public void Dispose()
    {
        if (_ownsHttpClient)
        {
            _httpClient.Dispose();
        }
    }

    private record ErrorResponse(string? Message, string? Code, Dictionary<string, string[]>? Errors);
}
