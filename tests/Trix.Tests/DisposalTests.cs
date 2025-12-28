using FluentAssertions;
using Trix;

namespace Trix.Tests;

/// <summary>
/// Tests for proper resource disposal and disposed state checking.
/// </summary>
public class DisposalTests
{
    [Fact]
    public void DisposedClient_AccessMemories_ThrowsObjectDisposedException()
    {
        // Arrange
        var client = new TrixClient("test_key", "https://api.test.com");
        client.Dispose();

        // Act & Assert
        var act = () => client.Memories;
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage("*TrixClient*");
    }

    [Fact]
    public void DisposedClient_AccessRelationships_ThrowsObjectDisposedException()
    {
        // Arrange
        var client = new TrixClient("test_key", "https://api.test.com");
        client.Dispose();

        // Act & Assert
        var act = () => client.Relationships;
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void DisposedClient_AccessClusters_ThrowsObjectDisposedException()
    {
        // Arrange
        var client = new TrixClient("test_key", "https://api.test.com");
        client.Dispose();

        // Act & Assert
        var act = () => client.Clusters;
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void DisposedClient_AccessSpaces_ThrowsObjectDisposedException()
    {
        // Arrange
        var client = new TrixClient("test_key", "https://api.test.com");
        client.Dispose();

        // Act & Assert
        var act = () => client.Spaces;
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void DisposedClient_AccessGraph_ThrowsObjectDisposedException()
    {
        // Arrange
        var client = new TrixClient("test_key", "https://api.test.com");
        client.Dispose();

        // Act & Assert
        var act = () => client.Graph;
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void DisposedClient_AccessSearch_ThrowsObjectDisposedException()
    {
        // Arrange
        var client = new TrixClient("test_key", "https://api.test.com");
        client.Dispose();

        // Act & Assert
        var act = () => client.Search;
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void DisposedClient_AccessWebhooks_ThrowsObjectDisposedException()
    {
        // Arrange
        var client = new TrixClient("test_key", "https://api.test.com");
        client.Dispose();

        // Act & Assert
        var act = () => client.Webhooks;
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void DisposedClient_AccessAgent_ThrowsObjectDisposedException()
    {
        // Arrange
        var client = new TrixClient("test_key", "https://api.test.com");
        client.Dispose();

        // Act & Assert
        var act = () => client.Agent;
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void DisposedClient_AccessFeedback_ThrowsObjectDisposedException()
    {
        // Arrange
        var client = new TrixClient("test_key", "https://api.test.com");
        client.Dispose();

        // Act & Assert
        var act = () => client.Feedback;
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void DisposedClient_AccessHighlights_ThrowsObjectDisposedException()
    {
        // Arrange
        var client = new TrixClient("test_key", "https://api.test.com");
        client.Dispose();

        // Act & Assert
        var act = () => client.Highlights;
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void DisposedClient_AccessJobs_ThrowsObjectDisposedException()
    {
        // Arrange
        var client = new TrixClient("test_key", "https://api.test.com");
        client.Dispose();

        // Act & Assert
        var act = () => client.Jobs;
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void DisposedClient_AccessFacts_ThrowsObjectDisposedException()
    {
        // Arrange
        var client = new TrixClient("test_key", "https://api.test.com");
        client.Dispose();

        // Act & Assert
        var act = () => client.Facts;
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void DisposedClient_AccessEntities_ThrowsObjectDisposedException()
    {
        // Arrange
        var client = new TrixClient("test_key", "https://api.test.com");
        client.Dispose();

        // Act & Assert
        var act = () => client.Entities;
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void DisposedClient_AccessEnrichments_ThrowsObjectDisposedException()
    {
        // Arrange
        var client = new TrixClient("test_key", "https://api.test.com");
        client.Dispose();

        // Act & Assert
        var act = () => client.Enrichments;
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void ActiveClient_AccessResources_Succeeds()
    {
        // Arrange
        using var client = new TrixClient("test_key", "https://api.test.com");

        // Act & Assert - should not throw
        client.Memories.Should().NotBeNull();
        client.Relationships.Should().NotBeNull();
        client.Clusters.Should().NotBeNull();
        client.Spaces.Should().NotBeNull();
        client.Graph.Should().NotBeNull();
        client.Search.Should().NotBeNull();
        client.Webhooks.Should().NotBeNull();
        client.Agent.Should().NotBeNull();
        client.Feedback.Should().NotBeNull();
        client.Highlights.Should().NotBeNull();
        client.Jobs.Should().NotBeNull();
        client.Facts.Should().NotBeNull();
        client.Entities.Should().NotBeNull();
        client.Enrichments.Should().NotBeNull();
    }

    [Fact]
    public void Dispose_MultipleTimes_DoesNotThrow()
    {
        // Arrange
        var client = new TrixClient("test_key", "https://api.test.com");

        // Act & Assert
        client.Dispose();
        var act = () => client.Dispose();
        act.Should().NotThrow();
    }
}
