﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Args = System.Collections.Generic.Dictionary<string, object>;

namespace SlackNet.WebApi
{
    public interface IUserGroupsApi
    {
        /// <summary>
        /// Used to create a User Group.
        /// </summary>
        /// <param name="name">A name for the User Group. Must be unique among User Groups.</param>
        /// <param name="channelIds">Channel IDs for which the User Group uses as a default.</param>
        /// <param name="description">A short description of the User Group.</param>
        /// <param name="handle">A mention handle. Must be unique among channels, users and User Groups.</param>
        /// <param name="includeCount">Include the number of users in each User Group.</param>
        /// <param name="cancellationToken"></param>
        Task<UserGroup> Create(
            string name,
            IEnumerable<string> channelIds = null,
            string description = null,
            string handle = null,
            bool includeCount = false,
            CancellationToken? cancellationToken = null
        );

        /// <summary>
        /// Disables an existing User Group.
        /// </summary>
        /// <param name="userGroupId">ID of the User Group to disable.</param>
        /// <param name="includeCount">Include the number of users in the User Group.</param>
        /// <param name="cancellationToken"></param>
        Task<UserGroup> Disable(string userGroupId, bool includeCount = false, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Enables a User Group which was previously disabled.
        /// </summary>
        /// <param name="userGroupId">ID of the User Group to enable.</param>
        /// <param name="includeCount">Include the number of users in the User Group.</param>
        /// <param name="cancellationToken"></param>
        Task<UserGroup> Enable(string userGroupId, bool includeCount = false, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Returns a list of all User Groups in the team. This can optionally include disabled User Groups.
        /// </summary>
        /// <param name="includeCount">Include the number of users in each User Group.</param>
        /// <param name="includeDisabled">Include disabled User Groups.</param>
        /// <param name="includeUsers">Include the list of users for each User Group.</param>
        /// <param name="cancellationToken"></param>
        Task<IList<UserGroup>> List(bool includeCount = false, bool includeDisabled = false, bool includeUsers = false, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Updates the properties of an existing User Group.
        /// </summary>
        /// <param name="userGroupId">ID of the User Group to update.</param>
        /// <param name="channelIds">Channel IDs for which the User Group uses as a default.</param>
        /// <param name="description">A short description of the User Group.</param>
        /// <param name="handle">A mention handle. Must be unique among channels, users and User Groups.</param>
        /// <param name="includeCount">Include the number of users in each User Group.</param>
        /// <param name="name">A name for the User Group. Must be unique among User Groups.</param>
        /// <param name="cancellationToken"></param>
        Task<UserGroup> Update(
            string userGroupId,
            IEnumerable<string> channelIds = null,
            string description = null,
            string handle = null,
            bool includeCount = false,
            string name = null,
            CancellationToken? cancellationToken = null
        );
    }

    public class UserGroupsApi : IUserGroupsApi
    {
        private readonly ISlackApiClient _client;
        public UserGroupsApi(ISlackApiClient client) => _client = client;

        /// <summary>
        /// Used to create a User Group.
        /// </summary>
        /// <param name="name">A name for the User Group. Must be unique among User Groups.</param>
        /// <param name="channelIds">Channel IDs for which the User Group uses as a default.</param>
        /// <param name="description">A short description of the User Group.</param>
        /// <param name="handle">A mention handle. Must be unique among channels, users and User Groups.</param>
        /// <param name="includeCount">Include the number of users in each User Group.</param>
        /// <param name="cancellationToken"></param>
        public async Task<UserGroup> Create(
            string name,
            IEnumerable<string> channelIds = null,
            string description = null,
            string handle = null,
            bool includeCount = false,
            CancellationToken? cancellationToken = null
        ) =>
            (await _client.Post<UserGroupResponse>("usergroups.create", new Args
                {
                    { "name", name },
                    { "channels", channelIds },
                    { "description", description },
                    { "handle", handle },
                    { "include_count", includeCount }
                }, cancellationToken).ConfigureAwait(false))
            .Usergroup;

        /// <summary>
        /// Disables an existing User Group.
        /// </summary>
        /// <param name="userGroupId">ID of the User Group to disable.</param>
        /// <param name="includeCount">Include the number of users in the User Group.</param>
        /// <param name="cancellationToken"></param>
        public async Task<UserGroup> Disable(string userGroupId, bool includeCount = false, CancellationToken? cancellationToken = null) =>
            (await _client.Post<UserGroupResponse>("usergroups.disable", new Args
                {
                    { "usergroup", userGroupId },
                    { "include_count", includeCount }
                }, cancellationToken).ConfigureAwait(false))
            .Usergroup;

        /// <summary>
        /// Enables a User Group which was previously disabled.
        /// </summary>
        /// <param name="userGroupId">ID of the User Group to enable.</param>
        /// <param name="includeCount">Include the number of users in the User Group.</param>
        /// <param name="cancellationToken"></param>
        public async Task<UserGroup> Enable(string userGroupId, bool includeCount = false, CancellationToken? cancellationToken = null) =>
            (await _client.Post<UserGroupResponse>("usergroups.enable", new Args
                {
                    { "usergroup", userGroupId },
                    { "include_count", includeCount }
                }, cancellationToken).ConfigureAwait(false))
            .Usergroup;

        /// <summary>
        /// Returns a list of all User Groups in the team. This can optionally include disabled User Groups.
        /// </summary>
        /// <param name="includeCount">Include the number of users in each User Group.</param>
        /// <param name="includeDisabled">Include disabled User Groups.</param>
        /// <param name="includeUsers">Include the list of users for each User Group.</param>
        /// <param name="cancellationToken"></param>
        public async Task<IList<UserGroup>> List(bool includeCount = false, bool includeDisabled = false, bool includeUsers = false, CancellationToken? cancellationToken = null) =>
            (await _client.Get<UserGroupListResponse>("usergroups.list", new Args
                {
                    { "include_count", includeCount },
                    { "include_disabled", includeDisabled },
                    { "include_users", includeUsers }
                }, cancellationToken).ConfigureAwait(false))
            .Usergroups;

        /// <summary>
        /// Updates the properties of an existing User Group.
        /// </summary>
        /// <param name="userGroupId">ID of the User Group to update.</param>
        /// <param name="channelIds">Channel IDs for which the User Group uses as a default.</param>
        /// <param name="description">A short description of the User Group.</param>
        /// <param name="handle">A mention handle. Must be unique among channels, users and User Groups.</param>
        /// <param name="includeCount">Include the number of users in each User Group.</param>
        /// <param name="name">A name for the User Group. Must be unique among User Groups.</param>
        /// <param name="cancellationToken"></param>
        public async Task<UserGroup> Update(
            string userGroupId,
            IEnumerable<string> channelIds = null,
            string description = null,
            string handle = null,
            bool includeCount = false,
            string name = null,
            CancellationToken? cancellationToken = null
        ) =>
            (await _client.Post<UserGroupResponse>("usergroups.update", new Args
                {
                    { "usergroup", userGroupId },
                    { "channels", channelIds },
                    { "description", description },
                    { "handle", handle },
                    { "include_count", includeCount },
                    { "name", name },
                }, cancellationToken).ConfigureAwait(false))
            .Usergroup;
    }
}