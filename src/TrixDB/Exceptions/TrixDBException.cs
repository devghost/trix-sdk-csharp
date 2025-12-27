using System.Net;

namespace TrixDB.Exceptions;

/// <summary>
/// Base exception for all TrixDB SDK errors.
/// </summary>
public class TrixDBException : Exception
{
    /// <summary>
    /// Gets the HTTP status code associated with this error, if any.
    /// </summary>
    public HttpStatusCode? StatusCode { get; }

    /// <summary>
    /// Gets the error code returned by the API, if any.
    /// </summary>
    public string? ErrorCode { get; }

    /// <summary>
    /// Gets the request ID for debugging purposes.
    /// </summary>
    public string? RequestId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TrixDBException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public TrixDBException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TrixDBException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public TrixDBException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TrixDBException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="errorCode">The error code from the API.</param>
    /// <param name="requestId">The request ID for debugging.</param>
    public TrixDBException(
        string message,
        HttpStatusCode statusCode,
        string? errorCode = null,
        string? requestId = null) : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        RequestId = requestId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TrixDBException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="innerException">The inner exception.</param>
    /// <param name="errorCode">The error code from the API.</param>
    /// <param name="requestId">The request ID for debugging.</param>
    public TrixDBException(
        string message,
        HttpStatusCode statusCode,
        Exception innerException,
        string? errorCode = null,
        string? requestId = null) : base(message, innerException)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        RequestId = requestId;
    }
}
