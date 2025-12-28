using System.Net;

namespace Trix.Exceptions;

/// <summary>
/// Exception thrown when a request times out.
/// </summary>
public class TrixTimeoutException : TrixException
{
    /// <summary>
    /// Gets the timeout duration that was exceeded.
    /// </summary>
    public TimeSpan? Timeout { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TrixTimeoutException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="timeout">The timeout duration that was exceeded.</param>
    public TrixTimeoutException(
        string message = "The request timed out.",
        TimeSpan? timeout = null)
        : base(message)
    {
        Timeout = timeout;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TrixTimeoutException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    /// <param name="timeout">The timeout duration that was exceeded.</param>
    public TrixTimeoutException(string message, Exception innerException, TimeSpan? timeout = null)
        : base(message, innerException)
    {
        Timeout = timeout;
    }
}
