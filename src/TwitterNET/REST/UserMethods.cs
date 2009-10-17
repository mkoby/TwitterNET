using System.Collections.Generic;

namespace TwitterNET
{
    public partial class Twitter
    {
        /// <summary>
        /// Gets a user's friends and their most recent statuses 
        /// Authenticated user by default, use StatusRequestOptions to be more specific.
        /// </summary>
        /// <param name="statusRequestOptions">
        /// Accepts either the UserID or ScreenName and/or the Page RequestOptions <see cref="StatusRequestOptions"/>
        /// </param>
        /// <returns>
        /// A list of users, with their most recent statuses <see cref="IList"/>
        /// </returns>
        public IList<IUser> GetUsersFriends(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/statuses/friends.xml";
            string resposneText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnUsers(resposneText);
        }

        /// <summary>
        /// Gets a user's follower's and their most recent statuses. 
        /// Authenticated user by default, use StatusRequestOptions to be more specific.
        /// </summary>
        /// <param name="statusRequestOptions">
        /// Accepts either the UserID or ScreenName and/or the Page RequestOptions <see cref="StatusRequestOptions"/>
        /// </param>
        /// <returns>
        /// A <see cref="IList"/>
        /// </returns>
        public IList<IUser> GetUsersFollowers(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/statuses/followers.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnUsers(responseText);
        }
    }

}