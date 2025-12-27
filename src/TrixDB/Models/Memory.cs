using System.Text.Json.Serialization;

namespace TrixDB.Models;

/// <summary>
/// Represents the type of memory content.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MemoryType
{
    /// <summary>Plain text content.</summary>
    [JsonPropertyName("text")] Text,

    /// <summary>Markdown formatted content.</summary>
    [JsonPropertyName("markdown")] Markdown,

    /// <summary>URL content.</summary>
    [JsonPropertyName("url")] Url,

    /// <summary>Audio content.</summary>
    [JsonPropertyName("audio")] Audio
}

/// <summary>
/// Represents the transcript status of audio memories.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TranscriptStatus
{
    /// <summary>Transcription is pending.</summary>
    [JsonPropertyName("pending")] Pending,

    /// <summary>Transcription is in progress.</summary>
    [JsonPropertyName("processing")] Processing,

    /// <summary>Transcription completed successfully.</summary>
    [JsonPropertyName("completed")] Completed,

    /// <summary>Transcription failed.</summary>
    [JsonPropertyName("failed")] Failed
}

/// <summary>
/// Represents a memory in TrixDB.
/// </summary>
public class Memory
{
    /// <summary>Gets or sets the unique identifier.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>Gets or sets the space this memory belongs to.</summary>
    [JsonPropertyName("spaceId")]
    public string? SpaceId { get; set; }

    /// <summary>Gets or sets the content type.</summary>
    [JsonPropertyName("type")]
    public MemoryType Type { get; set; } = MemoryType.Text;

    /// <summary>Gets or sets the content.</summary>
    [JsonPropertyName("content")]
    public required string Content { get; set; }

    /// <summary>Gets or sets the embedding vector.</summary>
    [JsonPropertyName("embedding")]
    public float[]? Embedding { get; set; }

    /// <summary>Gets or sets the tags.</summary>
    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }

    /// <summary>Gets or sets additional metadata.</summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>Gets or sets the creation timestamp.</summary>
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>Gets or sets the last update timestamp.</summary>
    [JsonPropertyName("updatedAt")]
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>Gets or sets the transcript status for audio memories.</summary>
    [JsonPropertyName("transcriptStatus")]
    public TranscriptStatus? TranscriptStatus { get; set; }
}

/// <summary>
/// Parameters for creating a new memory.
/// </summary>
public class CreateMemoryRequest
{
    /// <summary>Gets or sets the content.</summary>
    [JsonPropertyName("content")]
    public required string Content { get; set; }

    /// <summary>Gets or sets the content type.</summary>
    [JsonPropertyName("type")]
    public MemoryType Type { get; set; } = MemoryType.Text;

    /// <summary>Gets or sets the tags.</summary>
    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }

    /// <summary>Gets or sets additional metadata.</summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>Gets or sets the space ID.</summary>
    [JsonPropertyName("spaceId")]
    public string? SpaceId { get; set; }

    /// <summary>Gets or sets a pre-computed embedding.</summary>
    [JsonPropertyName("embedding")]
    public float[]? Embedding { get; set; }
}

/// <summary>
/// Parameters for updating an existing memory.
/// </summary>
public class UpdateMemoryRequest
{
    /// <summary>Gets or sets the content.</summary>
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    /// <summary>Gets or sets the tags.</summary>
    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }

    /// <summary>Gets or sets additional metadata.</summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>Gets or sets the embedding.</summary>
    [JsonPropertyName("embedding")]
    public float[]? Embedding { get; set; }
}

/// <summary>
/// Parameters for listing memories.
/// </summary>
public class ListMemoriesRequest
{
    /// <summary>Search query string.</summary>
    public string? Q { get; set; }

    /// <summary>Search mode (semantic, keyword, or hybrid).</summary>
    public SearchMode? Mode { get; set; }

    /// <summary>Maximum number of results to return.</summary>
    public int? Limit { get; set; }

    /// <summary>Page number for pagination.</summary>
    public int? Page { get; set; }

    /// <summary>Offset for pagination.</summary>
    public int? Offset { get; set; }

    /// <summary>Filter by tags.</summary>
    public List<string>? Tags { get; set; }

    /// <summary>Filter by memory type.</summary>
    public MemoryType? Type { get; set; }

    /// <summary>Filter by space ID.</summary>
    public string? SpaceId { get; set; }

    /// <summary>Sort field.</summary>
    public string? SortBy { get; set; }

    /// <summary>Sort order (asc or desc).</summary>
    public string? SortOrder { get; set; }
}

/// <summary>
/// Search mode for memory queries.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SearchMode
{
    /// <summary>Semantic search using embeddings.</summary>
    [JsonPropertyName("semantic")] Semantic,

    /// <summary>Keyword-based search.</summary>
    [JsonPropertyName("keyword")] Keyword,

    /// <summary>Hybrid search combining semantic and keyword.</summary>
    [JsonPropertyName("hybrid")] Hybrid
}
