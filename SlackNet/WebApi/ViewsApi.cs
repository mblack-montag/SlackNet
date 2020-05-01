using System.Threading;
using System.Threading.Tasks;
using Args = System.Collections.Generic.Dictionary<string, object>;

namespace SlackNet.WebApi
{
    public interface IViewsApi
    {
        /// <summary>
        /// Open a view for a user.
        /// </summary>
        /// <param name="triggerId">Exchange a trigger to post to the user.</param>
        /// <param name="view">A view payload.</param>
        /// <param name="cancellationToken"></param>
        Task<ViewResponse> Open(string triggerId, ViewDefinition view, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Create or update the view that comprises an app's Home tab for a specific user.
        /// </summary>
        /// <param name="userId">Id of the user you want publish a view to.</param>
        /// <param name="hash">A string that represents view state to protect against possible race conditions.</param>
        /// <param name="viewDefinition">A view payload.</param>
        /// <param name="cancellationToken"></param>
        Task<ViewResponse> Publish(string userId, HomeViewDefinition viewDefinition, string hash = null, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Push a new view onto the existing view stack by passing a view payload and a valid trigger ID
        /// generated from an interaction within the existing modal. The pushed view is added to the top of the stack,
        /// so the user will go back to the previous view after they complete or cancel the pushed view.
        /// </summary>
        /// <param name="triggerId">Exchange a trigger to post to the user.</param>
        /// <param name="view">A view payload.</param>
        /// <param name="cancellationToken"></param>
        Task<ViewResponse> Push(string triggerId, ViewDefinition view, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Update a view by passing a new view definition along with the <see cref="ViewInfo.Id"/> returned by <see cref="ViewsApi.Open"/> or the <see cref="ViewDefinition.ExternalId"/>.
        /// </summary>
        /// <param name="view">A view payload.</param>
        /// <param name="externalId">A unique identifier of the view set by the developer. Must be unique for all views on a team.</param>
        /// <param name="hash">A string that represents view state to protect against possible race conditions.</param>
        /// <param name="cancellationToken"></param>
        Task<ViewResponse> UpdateByExternalId(ViewDefinition view, string externalId, string hash = null, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Update a view by passing a new view definition along with the <see cref="ViewInfo.Id"/> returned by <see cref="ViewsApi.Open"/> or the <see cref="ViewDefinition.ExternalId"/>.
        /// </summary>
        /// <param name="view">A view payload.</param>
        /// <param name="hash">A string that represents view state to protect against possible race conditions.</param>
        /// <param name="viewId">A unique identifier of the view to be updated.</param>
        /// <param name="cancellationToken"></param>
        Task<ViewResponse> UpdateByViewId(ViewDefinition view, string viewId, string hash = null, CancellationToken? cancellationToken = null);
    }

    public class ViewsApi : IViewsApi
    {
        private readonly ISlackApiClient _client;
        public ViewsApi(ISlackApiClient client) => _client = client;

        /// <summary>
        /// Open a view for a user.
        /// </summary>
        /// <param name="triggerId">Exchange a trigger to post to the user.</param>
        /// <param name="view">A view payload.</param>
        /// <param name="cancellationToken"></param>
        public Task<ViewResponse> Open(string triggerId, ViewDefinition view, CancellationToken? cancellationToken = null) =>
            _client.Post<ViewResponse>("views.open", new Args
                {
                    { "trigger_id", triggerId },
                    { "view", view }
                }, cancellationToken);

        /// <summary>
        /// Create or update the view that comprises an app's Home tab for a specific user.
        /// </summary>
        /// <param name="userId">Id of the user you want publish a view to.</param>
        /// <param name="viewDefinition">A view payload.</param>
        /// <param name="hash">A string that represents view state to protect against possible race conditions.</param>
        /// <param name="cancellationToken"></param>
        public Task<ViewResponse> Publish(string userId, HomeViewDefinition viewDefinition, string hash = null, CancellationToken? cancellationToken = null) =>
            _client.Post<ViewResponse>("views.publish", new Args
                {
                    { "user_id", userId },
                    { "view", viewDefinition },
                    { "hash", hash }
                }, cancellationToken);

        /// <summary>
        /// Push a new view onto the existing view stack by passing a view payload and a valid trigger ID
        /// generated from an interaction within the existing modal. The pushed view is added to the top of the stack,
        /// so the user will go back to the previous view after they complete or cancel the pushed view.
        /// </summary>
        /// <param name="triggerId">Exchange a trigger to post to the user.</param>
        /// <param name="view">A view payload.</param>
        /// <param name="cancellationToken"></param>
        public Task<ViewResponse> Push(string triggerId, ViewDefinition view, CancellationToken? cancellationToken = null) =>
            _client.Post<ViewResponse>("views.push", new Args
                {
                    { "trigger_id", triggerId },
                    { "view", view }
                }, cancellationToken);

        /// <summary>
        /// Update a view by passing a new view definition along with the <see cref="ViewInfo.Id"/> returned by <see cref="Open"/> or the <see cref="ViewDefinition.ExternalId"/>.
        /// </summary>
        /// <param name="view">A view payload.</param>
        /// <param name="externalId">A unique identifier of the view set by the developer. Must be unique for all views on a team.</param>
        /// <param name="hash">A string that represents view state to protect against possible race conditions.</param>
        /// <param name="cancellationToken"></param>
        public Task<ViewResponse> UpdateByExternalId(ViewDefinition view, string externalId, string hash = null, CancellationToken? cancellationToken = null) =>
            _client.Post<ViewResponse>("views.update", new Args
                {
                    { "view", view },
                    { "external_id", externalId },
                    { "hash", hash }
                }, cancellationToken);

        /// <summary>
        /// Update a view by passing a new view definition along with the <see cref="ViewInfo.Id"/> returned by <see cref="Open"/> or the <see cref="ViewDefinition.ExternalId"/>.
        /// </summary>
        /// <param name="view">A view payload.</param>
        /// <param name="hash">A string that represents view state to protect against possible race conditions.</param>
        /// <param name="viewId">A unique identifier of the view to be updated.</param>
        /// <param name="cancellationToken"></param>
        public Task<ViewResponse> UpdateByViewId(ViewDefinition view, string viewId, string hash = null, CancellationToken? cancellationToken = null) =>
            _client.Post<ViewResponse>("views.update", new Args
                {
                    { "view", view },
                    { "view_id", viewId },
                    { "hash", hash }
                }, cancellationToken);
    }
}