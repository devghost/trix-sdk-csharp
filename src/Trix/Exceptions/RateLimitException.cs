using System.Net;

namespace Trix.Exceptions;

/// <summary>
/// Exception thrown when rate limit is exceeded (429 Too Many Requests).
/// </summary>
public class RateLimitException : TrixException
{
    /// <summary>
    /// Gets the number of seconds to wait before retrying.
    /// </summary>
    public int? RetryAfterSeconds { get; }

    /// <summary>
    /// Gets the time when the rate limit resets.
    /// </summary>
    public DateTimeOffset? ResetAt { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RateLimitException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="retryAfterSeconds">The number of seconds to wait before retrying.</param>
    /// <param name="resetAt">The time when the rate limit resets.</param>
    /// <param name="errorCode">The error code from the API.</param>
    /// <param name="requestId">The request ID for debugging.</param>
    public RateLimitException(
        string message = "Rate limit exceeded.",
        int? retryAfterSeconds = null,
        DateTimeOffset? resetAt = null,
        string? errorCode = null,
        string? requestId = null)
        : base(message, HttpStatusCode.TooManyRequests, errorCode, requestId)
    {
        RetryAfterSeconds = retryAfterSeconds;
        ResetAt = resetAt;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RateLimitException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public RateLimitException(string message, Exception innerException)
        : base(message, HttpStatusCode.TooManyRequests, innerException)
    {
    }
}
