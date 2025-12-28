using System.Net;

namespace Trix.Exceptions;

/// <summary>
/// Exception thrown when a server error occurs (5xx status codes).
/// </summary>
public class ServerException : TrixException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServerException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="errorCode">The error code from the API.</param>
    /// <param name="requestId">The request ID for debugging.</param>
    public ServerException(
        string message = "An internal server error occurred.",
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
        string? errorCode = null,
        string? requestId = null)
        : base(message, statusCode, errorCode, requestId)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public ServerException(string message, Exception innerException)
        : base(message, HttpStatusCode.InternalServerError, innerException)
    {
    }
}
