using System.Net;

namespace Trix.Exceptions;

/// <summary>
/// Exception thrown when validation fails (422 Unprocessable Entity).
/// </summary>
public class ValidationException : TrixException
{
    /// <summary>
    /// Gets the validation errors by field.
    /// </summary>
    public IReadOnlyDictionary<string, string[]>? Errors { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">The validation errors by field.</param>
    /// <param name="errorCode">The error code from the API.</param>
    /// <param name="requestId">The request ID for debugging.</param>
    public ValidationException(
        string message = "Validation failed.",
        IReadOnlyDictionary<string, string[]>? errors = null,
        string? errorCode = null,
        string? requestId = null)
        : base(message, HttpStatusCode.UnprocessableEntity, errorCode, requestId)
    {
        Errors = errors;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public ValidationException(string message, Exception innerException)
        : base(message, HttpStatusCode.UnprocessableEntity, innerException)
    {
    }
}
