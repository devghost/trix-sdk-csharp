using System.Net;

namespace Trix.Exceptions;

/// <summary>
/// Exception thrown when a resource is not found (404 Not Found).
/// </summary>
public class NotFoundException : TrixException
{
    /// <summary>
    /// Gets the type of resource that was not found.
    /// </summary>
    public string? ResourceType { get; }

    /// <summary>
    /// Gets the ID of the resource that was not found.
    /// </summary>
    public string? ResourceId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="resourceType">The type of resource that was not found.</param>
    /// <param name="resourceId">The ID of the resource that was not found.</param>
    /// <param name="errorCode">The error code from the API.</param>
    /// <param name="requestId">The request ID for debugging.</param>
    public NotFoundException(
        string message = "Resource not found.",
        string? resourceType = null,
        string? resourceId = null,
        string? errorCode = null,
        string? requestId = null)
        : base(message, HttpStatusCode.NotFound, errorCode, requestId)
    {
        ResourceType = resourceType;
        ResourceId = resourceId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public NotFoundException(string message, Exception innerException)
        : base(message, HttpStatusCode.NotFound, innerException)
    {
    }
}
