using TrixDB.Internal;
using TrixDB.Models;

namespace TrixDB.Resources;

/// <summary>
/// Provides methods for managing memories.
/// </summary>
public class MemoriesResource : BaseResource
{
    private const string BasePath = "/v1/memories";

    internal MemoriesResource(HttpPipeline pipeline) : base(pipeline) { }

    /// <summary>
    /// Creates a new memory.
    /// </summary>
    /// <param name="request">The memory creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created memory.</returns>
    public virtual async Task<Memory> CreateAsync(
        CreateMemoryRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await PostAsync<Memory>(BasePath, request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a memory by ID.
    /// </summary>
    /// <param name="id">The memory ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The memory.</returns>
    public virtual async Task<Memory> GetAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        return await GetAsync<Memory>($"{BasePath}/{id}", cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates an existing memory.
    /// </summary>
    /// <param name="id">The memory ID.</param>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated memory.</returns>
    public virtual async Task<Memory> UpdateAsync(
        string id,
        UpdateMemoryRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentNullException.ThrowIfNull(request);
        return await PatchAsync<Memory>($"{BasePath}/{id}", request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a memory.
    /// </summary>
    /// <param name="id">The memory ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public new virtual async Task DeleteAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        await base.DeleteAsync($"{BasePath}/{id}", cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Lists memories with optional filters.
    /// </summary>
    /// <param name="request">List parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of memories.</returns>
    public virtual async Task<PaginatedResponse<Memory>> ListAsync(
        ListMemoriesRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string?>();

        if (request != null)
        {
            if (request.Q != null) queryParams["q"] = request.Q;
            if (request.Mode != null) queryParams["mode"] = request.Mode.ToString()?.ToLowerInvariant();
            if (request.Limit != null) queryParams["limit"] = request.Limit.ToString();
            if (request.Page != null) queryParams["page"] = request.Page.ToString();
            if (request.Offset != null) queryParams["offset"] = request.Offset.ToString();
            if (request.Type != null) queryParams["type"] = request.Type.ToString()?.ToLowerInvariant();
            if (request.SpaceId != null) queryParams["spaceId"] = request.SpaceId;
            if (request.SortBy != null) queryParams["sortBy"] = request.SortBy;
            if (request.SortOrder != null) queryParams["sortOrder"] = request.SortOrder;
            if (request.Tags != null && request.Tags.Count > 0)
            {
                queryParams["tags"] = string.Join(",", request.Tags);
            }
        }

        return await GetAsync<PaginatedResponse<Memory>>(BasePath, queryParams, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates multiple memories in bulk.
    /// </summary>
    /// <param name="requests">The memory creation requests.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created memories.</returns>
    public virtual async Task<BulkResult> BulkCreateAsync(
        IEnumerable<CreateMemoryRequest> requests,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(requests);
        return await PostAsync<BulkResult>($"{BasePath}/bulk", new { memories = requests.ToList() }, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Iterates through all memories matching the criteria.
    /// </summary>
    /// <param name="request">Optional filter parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable of memories.</returns>
    public virtual async IAsyncEnumerable<Memory> ListAllAsync(
        ListMemoriesRequest? request = null,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        request ??= new ListMemoriesRequest();
        request.Limit ??= 100;
        request.Page ??= 1;

        while (true)
        {
            var response = await ListAsync(request, cancellationToken).ConfigureAwait(false);

            foreach (var memory in response.Data)
            {
                yield return memory;
            }

            if (response.Pagination?.HasMore != true || response.Data.Count == 0)
            {
                break;
            }

            request.Page++;
        }
    }
}

/// <summary>
/// Result of a bulk operation.
/// </summary>
public class BulkResult
{
    /// <summary>Gets or sets the number of successful operations.</summary>
    public int Success { get; set; }

    /// <summary>Gets or sets the number of failed operations.</summary>
    public int Failed { get; set; }

    /// <summary>Gets or sets any errors that occurred.</summary>
    public List<BulkError>? Errors { get; set; }
}

/// <summary>
/// Error from a bulk operation.
/// </summary>
public class BulkError
{
    /// <summary>Gets or sets the index of the failed item.</summary>
    public int Index { get; set; }

    /// <summary>Gets or sets the error message.</summary>
    public required string Message { get; set; }
}
