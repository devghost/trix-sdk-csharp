using System.Text.Json.Serialization;

namespace TrixDB.Models;

/// <summary>
/// Represents a named entity.
/// </summary>
public class Entity
{
    /// <summary>Gets or sets the unique identifier.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>Gets or sets the entity name.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>Gets or sets the entity type.</summary>
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    /// <summary>Gets or sets alternative names/aliases.</summary>
    [JsonPropertyName("aliases")]
    public List<string>? Aliases { get; set; }

    /// <summary>Gets or sets the entity description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Gets or sets entity properties.</summary>
    [JsonPropertyName("properties")]
    public Dictionary<string, object>? Properties { get; set; }

    /// <summary>Gets or sets linked memory IDs.</summary>
    [JsonPropertyName("memoryIds")]
    public List<string>? MemoryIds { get; set; }

    /// <summary>Gets or sets additional metadata.</summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>Gets or sets the space ID.</summary>
    [JsonPropertyName("spaceId")]
    public string? SpaceId { get; set; }

    /// <summary>Gets or sets the creation timestamp.</summary>
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>Gets or sets the last update timestamp.</summary>
    [JsonPropertyName("updatedAt")]
    public DateTimeOffset UpdatedAt { get; set; }
}

/// <summary>
/// Parameters for creating an entity.
/// </summary>
public class CreateEntityRequest
{
    /// <summary>Gets or sets the entity name.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>Gets or sets the entity type.</summary>
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    /// <summary>Gets or sets aliases.</summary>
    [JsonPropertyName("aliases")]
    public List<string>? Aliases { get; set; }

    /// <summary>Gets or sets the description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Gets or sets properties.</summary>
    [JsonPropertyName("properties")]
    public Dictionary<string, object>? Properties { get; set; }

    /// <summary>Gets or sets linked memory IDs.</summary>
    [JsonPropertyName("memoryIds")]
    public List<string>? MemoryIds { get; set; }

    /// <summary>Gets or sets additional metadata.</summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>Gets or sets the space ID.</summary>
    [JsonPropertyName("spaceId")]
    public string? SpaceId { get; set; }
}

/// <summary>
/// Parameters for updating an entity.
/// </summary>
public class UpdateEntityRequest
{
    /// <summary>Gets or sets the entity name.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>Gets or sets the entity type.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>Gets or sets aliases.</summary>
    [JsonPropertyName("aliases")]
    public List<string>? Aliases { get; set; }

    /// <summary>Gets or sets the description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Gets or sets properties.</summary>
    [JsonPropertyName("properties")]
    public Dictionary<string, object>? Properties { get; set; }

    /// <summary>Gets or sets additional metadata.</summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// An entity with a relevance score.
/// </summary>
public class ScoredEntity : Entity
{
    /// <summary>Gets or sets the relevance score.</summary>
    [JsonPropertyName("score")]
    public double Score { get; set; }
}

/// <summary>
/// Result of entity resolution.
/// </summary>
public class EntityResolutionResult
{
    /// <summary>Gets or sets the input text.</summary>
    [JsonPropertyName("text")]
    public required string Text { get; set; }

    /// <summary>Gets or sets the resolved entity.</summary>
    [JsonPropertyName("entity")]
    public Entity? Entity { get; set; }

    /// <summary>Gets or sets the confidence score.</summary>
    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }

    /// <summary>Gets or sets alternative matches.</summary>
    [JsonPropertyName("alternatives")]
    public List<EntityMatch>? Alternatives { get; set; }
}

/// <summary>
/// An entity match with confidence.
/// </summary>
public class EntityMatch
{
    /// <summary>Gets or sets the entity.</summary>
    [JsonPropertyName("entity")]
    public required Entity Entity { get; set; }

    /// <summary>Gets or sets the confidence.</summary>
    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }
}
