using TrixDB.Internal;
using TrixDB.Models;

namespace TrixDB.Resources;

/// <summary>
/// Resource for user feedback collection.
/// </summary>
public class FeedbackResource : BaseResource
{
    /// <summary>
    /// Initializes a new instance of the FeedbackResource.
    /// </summary>
    internal FeedbackResource(HttpPipeline pipeline) : base(pipeline)
    {
    }

    /// <summary>
    /// Submits detailed feedback on a memory.
    /// </summary>
    /// <param name="request">The feedback to submit.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The feedback result.</returns>
    public virtual async Task<FeedbackResult> SubmitAsync(
        SubmitFeedbackRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await PostAsync<FeedbackResult>("/v1/feedback", request, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Submits quick feedback (thumbs up/down) on a memory.
    /// </summary>
    /// <param name="request">The quick feedback request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The feedback result.</returns>
    public virtual async Task<FeedbackResult> QuickAsync(
        QuickFeedbackRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await PostAsync<FeedbackResult>("/v1/feedback/quick", request, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Submits feedback for multiple memories at once.
    /// </summary>
    /// <param name="request">The batch feedback request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The batch feedback result.</returns>
    public virtual async Task<BatchFeedbackResult> BatchAsync(
        BatchFeedbackRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await PostAsync<BatchFeedbackResult>("/v1/feedback/batch", request, cancellationToken)
            .ConfigureAwait(false);
    }
}
