using System.Net;
using FluentAssertions;
using TrixDB.Exceptions;

namespace TrixDB.Tests;

public class ExceptionTests
{
    [Fact]
    public void TrixDBException_WithMessage_SetsMessage()
    {
        // Arrange & Act
        var ex = new TrixDBException("Test error");

        // Assert
        ex.Message.Should().Be("Test error");
        ex.StatusCode.Should().BeNull();
        ex.ErrorCode.Should().BeNull();
        ex.RequestId.Should().BeNull();
    }

    [Fact]
    public void TrixDBException_WithStatusCode_SetsProperties()
    {
        // Arrange & Act
        var ex = new TrixDBException("Test error", HttpStatusCode.BadRequest, "ERR001", "req-123");

        // Assert
        ex.Message.Should().Be("Test error");
        ex.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ex.ErrorCode.Should().Be("ERR001");
        ex.RequestId.Should().Be("req-123");
    }

    [Fact]
    public void AuthenticationException_HasCorrectDefaults()
    {
        // Arrange & Act
        var ex = new AuthenticationException();

        // Assert
        ex.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        ex.Message.Should().Contain("Authentication");
    }

    [Fact]
    public void PermissionException_HasCorrectDefaults()
    {
        // Arrange & Act
        var ex = new PermissionException();

        // Assert
        ex.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        ex.Message.Should().Contain("Permission");
    }

    [Fact]
    public void NotFoundException_HasCorrectDefaults()
    {
        // Arrange & Act
        var ex = new NotFoundException("Memory not found", "Memory", "mem_123");

        // Assert
        ex.StatusCode.Should().Be(HttpStatusCode.NotFound);
        ex.ResourceType.Should().Be("Memory");
        ex.ResourceId.Should().Be("mem_123");
    }

    [Fact]
    public void ValidationException_HasCorrectDefaults()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "content", new[] { "Content is required" } }
        };

        // Act
        var ex = new ValidationException("Validation failed", errors);

        // Assert
        ex.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        ex.Errors.Should().ContainKey("content");
    }

    [Fact]
    public void RateLimitException_HasCorrectProperties()
    {
        // Arrange & Act
        var resetAt = DateTimeOffset.UtcNow.AddMinutes(1);
        var ex = new RateLimitException("Rate limit exceeded", 60, resetAt);

        // Assert
        ex.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
        ex.RetryAfterSeconds.Should().Be(60);
        ex.ResetAt.Should().Be(resetAt);
    }

    [Fact]
    public void ServerException_HasCorrectDefaults()
    {
        // Arrange & Act
        var ex = new ServerException();

        // Assert
        ex.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public void TrixDBTimeoutException_HasCorrectProperties()
    {
        // Arrange
        var timeout = TimeSpan.FromSeconds(30);

        // Act
        var ex = new TrixDBTimeoutException("Request timed out", timeout);

        // Assert
        ex.Timeout.Should().Be(timeout);
    }

    [Fact]
    public void NetworkException_WithInnerException_PreservesInner()
    {
        // Arrange
        var inner = new Exception("Connection refused");

        // Act
        var ex = new NetworkException("Network error", inner);

        // Assert
        ex.InnerException.Should().Be(inner);
    }
}
