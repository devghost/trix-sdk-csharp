using System.Text.Json;
using FluentAssertions;
using Trix.Models;

namespace Trix.Tests;

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

    [Fact]
    public void Webhook_Serialization_RoundTrip()
    {
        // Arrange
        var webhook = new Webhook
        {
            Id = "wh_123",
            Url = "https://example.com/webhook",
            Events = new List<string> { "memory.created", "memory.updated" },
            Secret = "secret123",
            Active = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var json = JsonSerializer.Serialize(webhook, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<Webhook>(json, JsonOptions);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Id.Should().Be(webhook.Id);
        deserialized.Url.Should().Be(webhook.Url);
        deserialized.Events.Should().BeEquivalentTo(webhook.Events);
        deserialized.Active.Should().BeTrue();
    }

    [Fact]
    public void Session_Serialization_RoundTrip()
    {
        // Arrange
        var session = new Session
        {
            Id = "sess_123",
            Name = "Test Session",
            Metadata = new Dictionary<string, object> { { "key", "value" } },
            StartedAt = DateTime.UtcNow
        };

        // Act
        var json = JsonSerializer.Serialize(session, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<Session>(json, JsonOptions);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Id.Should().Be(session.Id);
        deserialized.Name.Should().Be(session.Name);
    }

    [Fact]
    public void Job_Serialization_RoundTrip()
    {
        // Arrange
        var job = new Job
        {
            Id = "job_123",
            Name = "enrichment",
            Queue = "default",
            Status = JobStatus.Completed,
            Progress = 100,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var json = JsonSerializer.Serialize(job, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<Job>(json, JsonOptions);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Id.Should().Be(job.Id);
        deserialized.Status.Should().Be(JobStatus.Completed);
        deserialized.Progress.Should().Be(100);
    }

    [Fact]
    public void Highlight_Serialization_RoundTrip()
    {
        // Arrange
        var highlight = new Highlight
        {
            Id = "hl_123",
            MemoryId = "mem_123",
            Text = "Important text",
            StartOffset = 10,
            EndOffset = 25,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var json = JsonSerializer.Serialize(highlight, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<Highlight>(json, JsonOptions);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Id.Should().Be(highlight.Id);
        deserialized.Text.Should().Be(highlight.Text);
        deserialized.StartOffset.Should().Be(10);
        deserialized.EndOffset.Should().Be(25);
    }

    [Fact]
    public void Enrichment_Serialization_RoundTrip()
    {
        // Arrange
        var enrichment = new Enrichment
        {
            Type = EnrichmentType.Summary,
            Status = EnrichmentStatus.Completed,
            Data = new Dictionary<string, object> { { "summary", "Test summary" } },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var json = JsonSerializer.Serialize(enrichment, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<Enrichment>(json, JsonOptions);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Type.Should().Be(EnrichmentType.Summary);
        deserialized.Status.Should().Be(EnrichmentStatus.Completed);
    }

    [Fact]
    public void GraphTraversalResult_Serialization()
    {
        // Arrange
        var json = """
        {
            "nodes": [
                { "id": "node_1", "type": "memory", "label": "Node 1", "depth": 0 }
            ],
            "edges": [
                { "source": "node_1", "target": "node_2", "relationshipType": "related_to", "strength": 0.8 }
            ]
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<GraphTraversalResult>(json, JsonOptions);

        // Assert
        result.Should().NotBeNull();
        result!.Nodes.Should().HaveCount(1);
        result.Edges.Should().HaveCount(1);
        result.Nodes[0].Id.Should().Be("node_1");
        result.Edges[0].Strength.Should().Be(0.8);
    }

    [Fact]
    public void SimilarMemory_Serialization()
    {
        // Arrange
        var json = """
        {
            "memory": { "id": "mem_1", "content": "Test", "createdAt": "2025-01-01T00:00:00Z", "updatedAt": "2025-01-01T00:00:00Z" },
            "similarity": 0.95
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<SimilarMemory>(json, JsonOptions);

        // Assert
        result.Should().NotBeNull();
        result!.Memory.Id.Should().Be("mem_1");
        result.Similarity.Should().Be(0.95);
    }

    [Fact]
    public void EnrichmentType_EnumValues()
    {
        // Assert
        EnrichmentType.Summary.Should().Be(EnrichmentType.Summary);
        EnrichmentType.Keywords.Should().Be(EnrichmentType.Keywords);
        EnrichmentType.Entities.Should().Be(EnrichmentType.Entities);
        EnrichmentType.Topics.Should().Be(EnrichmentType.Topics);
        EnrichmentType.Sentiment.Should().Be(EnrichmentType.Sentiment);
    }

    [Fact]
    public void JobStatus_EnumValues()
    {
        // Assert
        JobStatus.Waiting.Should().Be(JobStatus.Waiting);
        JobStatus.Active.Should().Be(JobStatus.Active);
        JobStatus.Completed.Should().Be(JobStatus.Completed);
        JobStatus.Failed.Should().Be(JobStatus.Failed);
    }

    [Fact]
    public void FeedbackType_EnumValues()
    {
        // Assert
        FeedbackType.Positive.Should().Be(FeedbackType.Positive);
        FeedbackType.Negative.Should().Be(FeedbackType.Negative);
        FeedbackType.Neutral.Should().Be(FeedbackType.Neutral);
    }
}
