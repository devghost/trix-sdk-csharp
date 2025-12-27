using System.Net;

namespace TrixDB.Exceptions;

/// <summary>
/// Exception thrown when permission is denied (403 Forbidden).
/// </summary>
public class PermissionException : TrixDBException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errorCode">The error code from the API.</param>
    /// <param name="requestId">The request ID for debugging.</param>
    public PermissionException(
        string message = "Permission denied.",
        string? errorCode = null,
        string? requestId = null)
        : base(message, HttpStatusCode.Forbidden, errorCode, requestId)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public PermissionException(string message, Exception innerException)
        : base(message, HttpStatusCode.Forbidden, innerException)
    {
    }
}
