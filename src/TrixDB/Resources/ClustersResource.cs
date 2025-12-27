using TrixDB.Internal;
using TrixDB.Models;

namespace TrixDB.Resources;

/// <summary>
/// Provides methods for managing memory clusters.
/// </summary>
public class ClustersResource : BaseResource
{
    private const string BasePath = "/v1/clusters";

    internal ClustersResource(HttpPipeline pipeline) : base(pipeline) { }

    /// <summary>
    /// Creates a new cluster.
    /// </summary>
    public virtual async Task<Cluster> CreateAsync(
        CreateClusterRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await PostAsync<Cluster>(BasePath, request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a new cluster with the specified name.
    /// </summary>
    public virtual async Task<Cluster> CreateAsync(
        string name,
        string? description = null,
        List<string>? memoryIds = null,
        CancellationToken cancellationToken = default)
    {
        return await CreateAsync(new CreateClusterRequest
        {
            Name = name,
            Description = description,
            MemoryIds = memoryIds
        }, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a cluster by ID.
    /// </summary>
    public virtual async Task<Cluster> GetAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        return await GetAsync<Cluster>($"{BasePath}/{id}", cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates an existing cluster.
    /// </summary>
    public virtual async Task<Cluster> UpdateAsync(
        string id,
        UpdateClusterRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentNullException.ThrowIfNull(request);
        return await PatchAsync<Cluster>($"{BasePath}/{id}", request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a cluster.
    /// </summary>
    public new virtual async Task DeleteAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        await base.DeleteAsync($"{BasePath}/{id}", cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Lists clusters with optional filters.
    /// </summary>
    public virtual async Task<PaginatedResponse<Cluster>> ListAsync(
        ListClustersRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string?>();

        if (request != null)
        {
            if (request.Limit != null) queryParams["limit"] = request.Limit.ToString();
            if (request.Page != null) queryParams["page"] = request.Page.ToString();
            if (request.Offset != null) queryParams["offset"] = request.Offset.ToString();
            if (request.SpaceId != null) queryParams["spaceId"] = request.SpaceId;
            if (request.SortBy != null) queryParams["sortBy"] = request.SortBy;
            if (request.SortOrder != null) queryParams["sortOrder"] = request.SortOrder;
        }

        return await GetAsync<PaginatedResponse<Cluster>>(BasePath, queryParams, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds a memory to a cluster.
    /// </summary>
    public virtual async Task AddMemoryAsync(
        string clusterId,
        string memoryId,
        double confidence = 1.0,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(clusterId);
        ArgumentException.ThrowIfNullOrEmpty(memoryId);
        await PostAsync($"{BasePath}/{clusterId}/memories", new { memoryId, confidence }, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a memory from a cluster.
    /// </summary>
    public virtual async Task RemoveMemoryAsync(
        string clusterId,
        string memoryId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(clusterId);
        ArgumentException.ThrowIfNullOrEmpty(memoryId);
        await base.DeleteAsync($"{BasePath}/{clusterId}/memories/{memoryId}", cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Expands a cluster by finding similar memories.
    /// </summary>
    public virtual async Task<ExpandResult> ExpandAsync(
        string clusterId,
        int? limit = null,
        double? threshold = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(clusterId);
        var queryParams = BuildQueryParams(("limit", limit), ("threshold", threshold));
        return await GetAsync<ExpandResult>($"{BasePath}/{clusterId}/expand", queryParams, cancellationToken).ConfigureAwait(false);
    }
}
