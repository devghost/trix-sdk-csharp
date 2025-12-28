using System.Text.Json.Serialization;

namespace Trix.Models;

/// <summary>
/// Result of a bulk operation.
/// </summary>
public class BulkResult
{
    /// <summary>Gets or sets the number of successful operations.</summary>
    [JsonPropertyName("success")]
    public int Success { get; set; }

    /// <summary>Gets or sets the number of failed operations.</summary>
    [JsonPropertyName("failed")]
    public int Failed { get; set; }

    /// <summary>Gets or sets any errors that occurred.</summary>
    [JsonPropertyName("errors")]
    public List<BulkError>? Errors { get; set; }
}

/// <summary>
/// Error from a bulk operation.
/// </summary>
public class BulkError
{
    /// <summary>Gets or sets the index of the failed item.</summary>
    [JsonPropertyName("index")]
    public int Index { get; set; }

    /// <summary>Gets or sets the error message.</summary>
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}
