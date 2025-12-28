using FluentAssertions;
using Trix;

namespace Trix.Tests;

/// <summary>
/// Tests for security-related functionality including header validation.
/// </summary>
public class SecurityTests
{
    #region Custom Header Validation Tests

    [Theory]
    [InlineData("Authorization")]
    [InlineData("authorization")]
    [InlineData("AUTHORIZATION")]
    public void Validate_CustomHeaderWithAuthorization_ThrowsArgumentException(string headerName)
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            CustomHeaders = new Dictionary<string, string>
            {
                { headerName, "Bearer malicious_token" }
            }
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().Throw<ArgumentException>()
            .WithMessage("*restricted*header*");
    }

    [Theory]
    [InlineData("Host")]
    [InlineData("host")]
    public void Validate_CustomHeaderWithHost_ThrowsArgumentException(string headerName)
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            CustomHeaders = new Dictionary<string, string>
            {
                { headerName, "evil.com" }
            }
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().Throw<ArgumentException>()
            .WithMessage("*restricted*header*");
    }

    [Theory]
    [InlineData("Content-Length")]
    [InlineData("content-length")]
    [InlineData("Transfer-Encoding")]
    [InlineData("Connection")]
    public void Validate_CustomHeaderWithRestrictedHeader_ThrowsArgumentException(string headerName)
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            CustomHeaders = new Dictionary<string, string>
            {
                { headerName, "some value" }
            }
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().Throw<ArgumentException>()
            .WithMessage("*restricted*header*");
    }

    [Fact]
    public void Validate_CustomHeaderWithSafeHeader_Succeeds()
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            CustomHeaders = new Dictionary<string, string>
            {
                { "X-Custom-Header", "safe value" },
                { "X-Request-Trace-Id", "trace-123" }
            }
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_CustomHeadersNull_Succeeds()
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            CustomHeaders = null
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_CustomHeadersEmpty_Succeeds()
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            CustomHeaders = new Dictionary<string, string>()
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().NotThrow();
    }

    #endregion

    #region Timeout Validation Tests

    [Fact]
    public void Validate_TimeoutExceedsMaximum_ThrowsArgumentException()
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            Timeout = TimeSpan.FromHours(2) // Exceeds 1 hour max
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Timeout*exceed*");
    }

    [Fact]
    public void Validate_TimeoutAtMaximum_Succeeds()
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            Timeout = TimeSpan.FromHours(1) // Exactly at max
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_TimeoutAtMinimum_Succeeds()
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            Timeout = TimeSpan.FromSeconds(1) // Just above zero
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_NegativeTimeout_ThrowsArgumentException()
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            Timeout = TimeSpan.FromSeconds(-1)
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Timeout*positive*");
    }

    #endregion

    #region MaxRetries Validation Tests

    [Fact]
    public void Validate_MaxRetriesExceedsLimit_ThrowsArgumentException()
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            MaxRetries = 15 // Exceeds max of 10
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().Throw<ArgumentException>()
            .WithMessage("*MaxRetries*exceed*");
    }

    [Fact]
    public void Validate_MaxRetriesAtLimit_Succeeds()
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            MaxRetries = 10
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_MaxRetriesZero_Succeeds()
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            MaxRetries = 0 // No retries is valid
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().NotThrow();
    }

    #endregion

    #region BaseUrl Validation Tests

    [Fact]
    public void Validate_InvalidBaseUrl_ThrowsArgumentException()
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            BaseUrl = "not-a-valid-url"
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().Throw<ArgumentException>()
            .WithMessage("*BaseUrl*valid*URI*");
    }

    [Fact]
    public void Validate_RelativeBaseUrl_ThrowsArgumentException()
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            BaseUrl = "/api/v1"
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().Throw<ArgumentException>()
            .WithMessage("*BaseUrl*valid absolute URI*");
    }

    [Fact]
    public void Validate_ValidHttpsUrl_Succeeds()
    {
        // Arrange
        var options = new TrixClientOptions
        {
            ApiKey = "test_key",
            BaseUrl = "https://api.trixdb.com/v1"
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().NotThrow();
    }

    #endregion
}
