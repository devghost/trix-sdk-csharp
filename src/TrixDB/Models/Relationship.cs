using System.Text.Json.Serialization;

namespace TrixDB.Models;

/// <summary>
/// Represents a relationship between two memories.
/// </summary>
public class Relationship
{
    /// <summary>Gets or sets the unique identifier.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>Gets or sets the source memory ID.</summary>
    [JsonPropertyName("sourceId")]
    public required string SourceId { get; set; }

    /// <summary>Gets or sets the target memory ID.</summary>
    [JsonPropertyName("targetId")]
    public required string TargetId { get; set; }

    /// <summary>Gets or sets the relationship type.</summary>
    [JsonPropertyName("relationshipType")]
    public required string RelationshipType { get; set; }

    /// <summary>Gets or sets the relationship strength (0-1).</summary>
    [JsonPropertyName("strength")]
    public double Strength { get; set; } = 1.0;

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
/// Parameters for creating a relationship.
/// </summary>
public class CreateRelationshipRequest
{
    /// <summary>Gets or sets the source memory ID.</summary>
    [JsonPropertyName("sourceId")]
    public required string SourceId { get; set; }

    /// <summary>Gets or sets the target memory ID.</summary>
    [JsonPropertyName("targetId")]
    public required string TargetId { get; set; }

    /// <summary>Gets or sets the relationship type.</summary>
    [JsonPropertyName("relationshipType")]
    public required string RelationshipType { get; set; }

    /// <summary>Gets or sets the relationship strength.</summary>
    [JsonPropertyName("strength")]
    public double Strength { get; set; } = 1.0;

    /// <summary>Gets or sets additional metadata.</summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// Parameters for updating a relationship.
/// </summary>
public class UpdateRelationshipRequest
{
    /// <summary>Gets or sets the relationship type.</summary>
    [JsonPropertyName("relationshipType")]
    public string? RelationshipType { get; set; }

    /// <summary>Gets or sets the relationship strength.</summary>
    [JsonPropertyName("strength")]
    public double? Strength { get; set; }

    /// <summary>Gets or sets additional metadata.</summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// Common relationship types.
/// </summary>
public static class RelationshipTypes
{
    public const string RelatedTo = "related_to";
    public const string Supports = "supports";
    public const string Contradicts = "contradicts";
    public const string References = "references";
    public const string Extends = "extends";
    public const string DependsOn = "depends_on";
    public const string Contains = "contains";
    public const string PartOf = "part_of";
    public const string Causes = "causes";
    public const string CausedBy = "caused_by";
}
