using FluentAssertions;
using TrixDB;

namespace TrixDB.Tests;

public class OptionsTests
{
    [Fact]
    public void Validate_WithApiKey_Succeeds()
    {
        // Arrange
        var options = new TrixDBClientOptions { ApiKey = "test_key" };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithJwtToken_Succeeds()
    {
        // Arrange
        var options = new TrixDBClientOptions { JwtToken = "test_token" };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithoutCredentials_ThrowsArgumentException()
    {
        // Arrange
        var options = new TrixDBClientOptions();

        // Act & Assert
        var act = () => options.Validate();
        act.Should().Throw<ArgumentException>()
           .WithMessage("*ApiKey*JwtToken*");
    }

    [Fact]
    public void Validate_WithHttpUrl_ThrowsArgumentException()
    {
        // Arrange
        var options = new TrixDBClientOptions
        {
            ApiKey = "test_key",
            BaseUrl = "http://api.example.com"
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().Throw<ArgumentException>()
           .WithMessage("*HTTPS*");
    }

    [Fact]
    public void Validate_WithHttpUrlAndAllowInsecure_Succeeds()
    {
        // Arrange
        var options = new TrixDBClientOptions
        {
            ApiKey = "test_key",
            BaseUrl = "http://localhost",
            AllowInsecure = true
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithNegativeRetries_ThrowsArgumentException()
    {
        // Arrange
        var options = new TrixDBClientOptions
        {
            ApiKey = "test_key",
            MaxRetries = -1
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().Throw<ArgumentException>()
           .WithMessage("*MaxRetries*");
    }

    [Fact]
    public void Validate_WithZeroTimeout_ThrowsArgumentException()
    {
        // Arrange
        var options = new TrixDBClientOptions
        {
            ApiKey = "test_key",
            Timeout = TimeSpan.Zero
        };

        // Act & Assert
        var act = () => options.Validate();
        act.Should().Throw<ArgumentException>()
           .WithMessage("*Timeout*");
    }

    [Fact]
    public void FromEnvironment_WithoutEnvVar_ThrowsInvalidOperationException()
    {
        // Arrange - ensure env var is not set
        Environment.SetEnvironmentVariable("TRIXDB_API_KEY", null);

        // Act & Assert
        var act = () => TrixDBClientOptions.FromEnvironment();
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("*TRIXDB_API_KEY*");
    }

    [Fact]
    public void FromEnvironment_WithEnvVar_ReturnsOptions()
    {
        // Arrange
        Environment.SetEnvironmentVariable("TRIXDB_API_KEY", "test_key_from_env");
        Environment.SetEnvironmentVariable("TRIXDB_BASE_URL", "https://custom.api.com");

        try
        {
            // Act
            var options = TrixDBClientOptions.FromEnvironment();

            // Assert
            options.ApiKey.Should().Be("test_key_from_env");
            options.BaseUrl.Should().Be("https://custom.api.com");
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("TRIXDB_API_KEY", null);
            Environment.SetEnvironmentVariable("TRIXDB_BASE_URL", null);
        }
    }

    [Fact]
    public void DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var options = new TrixDBClientOptions { ApiKey = "test" };

        // Assert
        options.BaseUrl.Should().Be("https://api.trixdb.com");
        options.Timeout.Should().Be(TimeSpan.FromSeconds(30));
        options.MaxRetries.Should().Be(3);
        options.AllowInsecure.Should().BeFalse();
    }
}
