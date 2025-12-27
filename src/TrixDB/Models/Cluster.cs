using System.Text.Json.Serialization;

namespace TrixDB.Models;

/// <summary>
/// Represents a cluster of related memories.
/// </summary>
public class Cluster
{
    /// <summary>Gets or sets the unique identifier.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>Gets or sets the space this cluster belongs to.</summary>
    [JsonPropertyName("spaceId")]
    public string? SpaceId { get; set; }

    /// <summary>Gets or sets the cluster name.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>Gets or sets the cluster description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Gets or sets the memory IDs in this cluster.</summary>
    [JsonPropertyName("memoryIds")]
    public List<string>? MemoryIds { get; set; }

    /// <summary>Gets or sets the centroid vector.</summary>
    [JsonPropertyName("centroid")]
    public float[]? Centroid { get; set; }

    /// <summary>Gets or sets additional metadata.</summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>Gets or sets the creation timestamp.</summary>
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>Gets or sets the last update timestamp.</summary>
    [JsonPropertyName("updatedAt")]
    public DateTimeOffset UpdatedAt { get; set; }
}

/// <summary>
/// Parameters for creating a cluster.
/// </summary>
public class CreateClusterRequest
{
    /// <summary>Gets or sets the cluster name.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>Gets or sets the cluster description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Gets or sets the initial memory IDs.</summary>
    [JsonPropertyName("memoryIds")]
    public List<string>? MemoryIds { get; set; }

    /// <summary>Gets or sets the space ID.</summary>
    [JsonPropertyName("spaceId")]
    public string? SpaceId { get; set; }

    /// <summary>Gets or sets additional metadata.</summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// Parameters for updating a cluster.
/// </summary>
public class UpdateClusterRequest
{
    /// <summary>Gets or sets the cluster name.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>Gets or sets the cluster description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Gets or sets additional metadata.</summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// Parameters for listing clusters.
/// </summary>
public class ListClustersRequest
{
    /// <summary>Maximum number of results.</summary>
    public int? Limit { get; set; }

    /// <summary>Page number.</summary>
    public int? Page { get; set; }

    /// <summary>Offset for pagination.</summary>
    public int? Offset { get; set; }

    /// <summary>Filter by space ID.</summary>
    public string? SpaceId { get; set; }

    /// <summary>Sort field.</summary>
    public string? SortBy { get; set; }

    /// <summary>Sort order.</summary>
    public string? SortOrder { get; set; }
}

/// <summary>
/// Result of cluster expansion.
/// </summary>
public class ExpandResult
{
    /// <summary>Gets or sets the cluster ID.</summary>
    [JsonPropertyName("clusterId")]
    public required string ClusterId { get; set; }

    /// <summary>Gets or sets the suggested new memories.</summary>
    [JsonPropertyName("newMemories")]
    public required List<MemorySuggestion> NewMemories { get; set; }
}

/// <summary>
/// A memory suggestion with confidence score.
/// </summary>
public class MemorySuggestion
{
    /// <summary>Gets or sets the memory ID.</summary>
    [JsonPropertyName("memoryId")]
    public required string MemoryId { get; set; }

    /// <summary>Gets or sets the confidence score.</summary>
    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }
}
