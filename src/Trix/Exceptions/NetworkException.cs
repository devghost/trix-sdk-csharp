namespace Trix.Exceptions;

/// <summary>
/// Exception thrown when a network error occurs.
/// </summary>
public class NetworkException : TrixException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NetworkException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public NetworkException(string message = "A network error occurred.")
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NetworkException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public NetworkException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
