using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Trix.Internal;
using Trix.Resources;

namespace Trix;

/// <summary>
/// The main Trix client for interacting with the Trix API.
/// </summary>
/// <remarks>
/// This client is thread-safe and can be used as a singleton.
/// It implements <see cref="IDisposable"/> to clean up resources.
/// </remarks>
/// <example>
/// Using API key:
/// <code>
/// using var client = new TrixClient("your_api_key");
/// var memory = await client.Memories.CreateAsync(new CreateMemoryRequest { Content = "Hello, Trix!" });
/// </code>
/// </example>
public sealed class TrixClient : IDisposable
{
    private readonly HttpPipeline _pipeline;
    private readonly ILogger<TrixClient> _logger;
    private bool _disposed;

    /// <summary>
    /// Gets the SDK version.
    /// </summary>
    public static string Version => HttpPipeline.SdkVersion;

    /// <summary>
    /// Gets the API version.
    /// </summary>
    public static string ApiVersion => HttpPipeline.ApiVersion;

    /// <summary>
    /// Gets the memories resource for managing memories.
    /// </summary>
    public MemoriesResource Memories { get; }

    /// <summary>
    /// Gets the relationships resource for managing relationships.
    /// </summary>
    public RelationshipsResource Relationships { get; }

    /// <summary>
    /// Gets the clusters resource for managing clusters.
    /// </summary>
    public ClustersResource Clusters { get; }

    /// <summary>
    /// Gets the spaces resource for managing spaces.
    /// </summary>
    public SpacesResource Spaces { get; }

    /// <summary>
    /// Gets the graph resource for knowledge graph traversal.
    /// </summary>
    public GraphResource Graph { get; }

    /// <summary>
    /// Gets the search resource for semantic search operations.
    /// </summary>
    public SearchResource Search { get; }

    /// <summary>
    /// Gets the webhooks resource for managing webhooks.
    /// </summary>
    public WebhooksResource Webhooks { get; }

    /// <summary>
    /// Gets the agent resource for agent sessions and core memory.
    /// </summary>
    public AgentResource Agent { get; }

    /// <summary>
    /// Gets the feedback resource for submitting feedback.
    /// </summary>
    public FeedbackResource Feedback { get; }

    /// <summary>
    /// Gets the highlights resource for managing highlights.
    /// </summary>
    public HighlightsResource Highlights { get; }

    /// <summary>
    /// Gets the jobs resource for background job management.
    /// </summary>
    public JobsResource Jobs { get; }

    /// <summary>
    /// Gets the facts resource for knowledge graph facts.
    /// </summary>
    public FactsResource Facts { get; }

    /// <summary>
    /// Gets the entities resource for named entity management.
    /// </summary>
    public EntitiesResource Entities { get; }

    /// <summary>
    /// Gets the enrichments resource for memory enrichments.
    /// </summary>
    public EnrichmentsResource Enrichments { get; }

    /// <summary>
    /// Creates a new Trix client with the specified API key.
    /// </summary>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="baseUrl">Optional custom base URL.</param>
    public TrixClient(string apiKey, string? baseUrl = null)
        : this(new TrixClientOptions
        {
            ApiKey = apiKey,
            BaseUrl = baseUrl ?? TrixClientOptions.DefaultBaseUrl
        })
    {
    }

    /// <summary>
    /// Creates a new Trix client with the specified options.
    /// </summary>
    /// <param name="options">The client configuration options.</param>
    /// <exception cref="ArgumentNullException">If options is null.</exception>
    /// <exception cref="ArgumentException">If options are invalid.</exception>
    public TrixClient(TrixClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var loggerFactory = options.LoggerFactory ?? NullLoggerFactory.Instance;
        _logger = loggerFactory.CreateLogger<TrixClient>();

        _pipeline = new HttpPipeline(options);

        // Initialize resources
        Memories = new MemoriesResource(_pipeline);
        Relationships = new RelationshipsResource(_pipeline);
        Clusters = new ClustersResource(_pipeline);
        Spaces = new SpacesResource(_pipeline);
        Graph = new GraphResource(_pipeline);
        Search = new SearchResource(_pipeline);
        Webhooks = new WebhooksResource(_pipeline);
        Agent = new AgentResource(_pipeline);
        Feedback = new FeedbackResource(_pipeline);
        Highlights = new HighlightsResource(_pipeline);
        Jobs = new JobsResource(_pipeline);
        Facts = new FactsResource(_pipeline);
        Entities = new EntitiesResource(_pipeline);
        Enrichments = new EnrichmentsResource(_pipeline);

        _logger.LogInformation("TrixClient initialized (SDK v{Version})", Version);
    }

    /// <summary>
    /// Creates a Trix client from environment variables.
    /// Reads TRIX_API_KEY and optionally TRIX_BASE_URL.
    /// </summary>
    /// <returns>A configured Trix client.</returns>
    /// <exception cref="InvalidOperationException">If TRIX_API_KEY is not set.</exception>
    public static TrixClient FromEnvironment()
    {
        var options = TrixClientOptions.FromEnvironment();
        return new TrixClient(options);
    }

    /// <summary>
    /// Creates a Trix client from environment variables with custom options.
    /// </summary>
    /// <param name="configure">Action to configure additional options.</param>
    /// <returns>A configured Trix client.</returns>
    public static TrixClient FromEnvironment(Action<TrixClientOptions> configure)
    {
        var options = TrixClientOptions.FromEnvironment();
        configure(options);
        return new TrixClient(options);
    }

    /// <summary>
    /// Disposes the client and releases resources.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;

        _pipeline.Dispose();
        _disposed = true;

        _logger.LogDebug("TrixClient disposed");
    }

    /// <summary>
    /// Throws if the client has been disposed.
    /// </summary>
    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }
}
