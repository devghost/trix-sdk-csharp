using System.Text.Json;
using FluentAssertions;
using TrixDB.Models;

namespace TrixDB.Tests;

public class ModelTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public void Memory_Serialization_RoundTrip()
    {
        // Arrange
        var memory = new Memory
        {
            Id = "mem_123",
            Content = "Test content",
            Type = MemoryType.Text,
            Tags = new List<string> { "tag1", "tag2" },
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        // Act
        var json = JsonSerializer.Serialize(memory, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<Memory>(json, JsonOptions);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Id.Should().Be(memory.Id);
        deserialized.Content.Should().Be(memory.Content);
        deserialized.Type.Should().Be(MemoryType.Text);
        deserialized.Tags.Should().BeEquivalentTo(memory.Tags);
    }

    [Fact]
    public void CreateMemoryRequest_Serialization()
    {
        // Arrange
        var request = new CreateMemoryRequest
        {
            Content = "Test content",
            Type = MemoryType.Markdown,
            Tags = new List<string> { "test" }
        };

        // Act
        var json = JsonSerializer.Serialize(request, JsonOptions);

        // Assert
        json.Should().Contain("\"content\":\"Test content\"");
        json.Should().Contain("\"type\":\"Markdown\"");
    }

    [Fact]
    public void Relationship_Serialization_RoundTrip()
    {
        // Arrange
        var relationship = new Relationship
        {
            Id = "rel_123",
            SourceId = "mem_1",
            TargetId = "mem_2",
            RelationshipType = RelationshipTypes.RelatedTo,
            Strength = 0.8,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        // Act
        var json = JsonSerializer.Serialize(relationship, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<Relationship>(json, JsonOptions);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Id.Should().Be(relationship.Id);
        deserialized.SourceId.Should().Be(relationship.SourceId);
        deserialized.TargetId.Should().Be(relationship.TargetId);
        deserialized.Strength.Should().Be(0.8);
    }

    [Fact]
    public void Cluster_Serialization_RoundTrip()
    {
        // Arrange
        var cluster = new Cluster
        {
            Id = "cluster_123",
            Name = "Test Cluster",
            Description = "A test cluster",
            MemoryIds = new List<string> { "mem_1", "mem_2" },
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        // Act
        var json = JsonSerializer.Serialize(cluster, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<Cluster>(json, JsonOptions);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Id.Should().Be(cluster.Id);
        deserialized.Name.Should().Be(cluster.Name);
        deserialized.MemoryIds.Should().BeEquivalentTo(cluster.MemoryIds);
    }

    [Fact]
    public void Space_Serialization_RoundTrip()
    {
        // Arrange
        var space = new Space
        {
            Id = "space_123",
            Name = "Test Space",
            Description = "A test space",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        // Act
        var json = JsonSerializer.Serialize(space, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<Space>(json, JsonOptions);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Id.Should().Be(space.Id);
        deserialized.Name.Should().Be(space.Name);
    }

    [Fact]
    public void Fact_Serialization_RoundTrip()
    {
        // Arrange
        var fact = new Fact
        {
            Id = "fact_123",
            Subject = "Albert Einstein",
            Predicate = "was_born_in",
            Object = "Ulm, Germany",
            Confidence = 0.95,
            SubjectType = FactNodeType.Entity,
            ObjectType = FactNodeType.Text,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        // Act
        var json = JsonSerializer.Serialize(fact, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<Fact>(json, JsonOptions);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Subject.Should().Be(fact.Subject);
        deserialized.Predicate.Should().Be(fact.Predicate);
        deserialized.Object.Should().Be(fact.Object);
        deserialized.Confidence.Should().Be(0.95);
    }

    [Fact]
    public void Entity_Serialization_RoundTrip()
    {
        // Arrange
        var entity = new Entity
        {
            Id = "ent_123",
            Name = "Albert Einstein",
            Type = "person",
            Aliases = new List<string> { "Einstein", "A. Einstein" },
            Properties = new Dictionary<string, object>
            {
                { "birthYear", 1879 }
            },
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        // Act
        var json = JsonSerializer.Serialize(entity, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<Entity>(json, JsonOptions);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Name.Should().Be(entity.Name);
        deserialized.Type.Should().Be("person");
        deserialized.Aliases.Should().BeEquivalentTo(entity.Aliases);
    }

    [Fact]
    public void PaginatedResponse_Serialization()
    {
        // Arrange
        var json = """
        {
            "data": [
                { "id": "mem_1", "content": "Content 1", "type": "text", "createdAt": "2025-01-01T00:00:00Z", "updatedAt": "2025-01-01T00:00:00Z" },
                { "id": "mem_2", "content": "Content 2", "type": "text", "createdAt": "2025-01-01T00:00:00Z", "updatedAt": "2025-01-01T00:00:00Z" }
            ],
            "pagination": {
                "total": 100,
                "page": 1,
                "limit": 10,
                "hasMore": true
            }
        }
        """;

        // Act
        var response = JsonSerializer.Deserialize<PaginatedResponse<Memory>>(json, JsonOptions);

        // Assert
        response.Should().NotBeNull();
        response!.Data.Should().HaveCount(2);
        response.Pagination.Should().NotBeNull();
        response.Pagination!.Total.Should().Be(100);
        response.Pagination.HasMore.Should().BeTrue();
    }

    [Fact]
    public void MemoryType_EnumValues()
    {
        // Assert
        MemoryType.Text.Should().Be(MemoryType.Text);
        MemoryType.Markdown.Should().Be(MemoryType.Markdown);
        MemoryType.Url.Should().Be(MemoryType.Url);
        MemoryType.Audio.Should().Be(MemoryType.Audio);
    }

    [Fact]
    public void SearchMode_EnumValues()
    {
        // Assert
        SearchMode.Semantic.Should().Be(SearchMode.Semantic);
        SearchMode.Keyword.Should().Be(SearchMode.Keyword);
        SearchMode.Hybrid.Should().Be(SearchMode.Hybrid);
    }
}
