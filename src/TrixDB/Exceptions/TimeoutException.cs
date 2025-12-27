using System.Net;

namespace TrixDB.Exceptions;

/// <summary>
/// Exception thrown when a request times out.
/// </summary>
public class TrixDBTimeoutException : TrixDBException
{
    /// <summary>
    /// Gets the timeout duration that was exceeded.
    /// </summary>
    public TimeSpan? Timeout { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TrixDBTimeoutException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="timeout">The timeout duration that was exceeded.</param>
    public TrixDBTimeoutException(
        string message = "The request timed out.",
        TimeSpan? timeout = null)
        : base(message)
    {
        Timeout = timeout;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TrixDBTimeoutException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    /// <param name="timeout">The timeout duration that was exceeded.</param>
    public TrixDBTimeoutException(string message, Exception innerException, TimeSpan? timeout = null)
        : base(message, innerException)
    {
        Timeout = timeout;
    }
}
