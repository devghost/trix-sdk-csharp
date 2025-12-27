using FluentAssertions;
using TrixDB;

namespace TrixDB.Tests;

public class ClientTests
{
    [Fact]
    public void Constructor_WithApiKey_CreatesClient()
    {
        // Arrange & Act
        using var client = new TrixDBClient("test_api_key", "https://api.test.com");

        // Assert
        client.Should().NotBeNull();
        client.Memories.Should().NotBeNull();
        client.Relationships.Should().NotBeNull();
        client.Clusters.Should().NotBeNull();
        client.Spaces.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithOptions_CreatesClient()
    {
        // Arrange
        var options = new TrixDBClientOptions
        {
            ApiKey = "test_api_key",
            BaseUrl = "https://api.test.com",
            Timeout = TimeSpan.FromSeconds(60),
            MaxRetries = 5
        };

        // Act
        using var client = new TrixDBClient(options);

        // Assert
        client.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNullApiKey_ThrowsException()
    {
        // Arrange
        var options = new TrixDBClientOptions { BaseUrl = "https://api.test.com" };

        // Act & Assert
        var act = () => new TrixDBClient(options);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WithHttpUrl_ThrowsException()
    {
        // Arrange
        var options = new TrixDBClientOptions
        {
            ApiKey = "test_api_key",
            BaseUrl = "http://api.test.com"
        };

        // Act & Assert
        var act = () => new TrixDBClient(options);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WithHttpUrlAndAllowInsecure_Succeeds()
    {
        // Arrange
        var options = new TrixDBClientOptions
        {
            ApiKey = "test_api_key",
            BaseUrl = "http://localhost:8080",
            AllowInsecure = true
        };

        // Act
        using var client = new TrixDBClient(options);

        // Assert
        client.Should().NotBeNull();
    }

    [Fact]
    public void Version_ReturnsVersion()
    {
        // Act
        var version = TrixDBClient.Version;

        // Assert
        version.Should().NotBeNullOrEmpty();
        version.Should().Be("1.0.0");
    }

    [Fact]
    public void ApiVersion_ReturnsApiVersion()
    {
        // Act
        var apiVersion = TrixDBClient.ApiVersion;

        // Assert
        apiVersion.Should().NotBeNullOrEmpty();
        apiVersion.Should().Be("v1");
    }

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        // Arrange
        var client = new TrixDBClient("test_api_key", "https://api.test.com");

        // Act & Assert - should not throw
        client.Dispose();
        client.Dispose();
    }
}
