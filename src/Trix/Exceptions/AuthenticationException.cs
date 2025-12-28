using System.Net;

namespace Trix.Exceptions;

/// <summary>
/// Exception thrown when authentication fails (401 Unauthorized).
/// </summary>
public class AuthenticationException : TrixException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errorCode">The error code from the API.</param>
    /// <param name="requestId">The request ID for debugging.</param>
    public AuthenticationException(
        string message = "Authentication failed. Please check your API key.",
        string? errorCode = null,
        string? requestId = null)
        : base(message, HttpStatusCode.Unauthorized, errorCode, requestId)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public AuthenticationException(string message, Exception innerException)
        : base(message, HttpStatusCode.Unauthorized, innerException)
    {
    }
}
