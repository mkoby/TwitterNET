using System;
using System.Collections.Generic;

namespace TwitterNET
{
    public partial class Twitter
    {
        /// <summary>
        /// Gets a user's friends and their most recent statuses 
        /// Authenticated user by default, use StatusRequestOptions to be more specific.
        /// </summary>
        /// <param name="statusRequestOptions">Accepted options: UserID, ScreenName</param>
        /// <returns>List of Users who the supplied user is following. Requires authentication if supplied user is protected</returns>
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
        /// <param name="statusRequestOptions">Accepted options: UserID, ScreenName</param>
        /// <returns>List of User who are following the supplied user. Requires authentication if supplied user is protected</returns>
        public IList<IUser> GetUsersFollowers(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/statuses/followers.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnUsers(responseText);
        }

        /// <summary>
        /// extended information of a given user and their most recent status..
        /// </summary>
        /// <param name="statusRequestOptions">Accepted options: UserID, ScreenName</param>
        /// <returns>User information and most recent status of requested user</returns>
        public IUser GetSingleUser(StatusRequestOptions statusRequestOptions)
        {
            IUser output = null;
            string apiURL = "http://twitter.com/users/show.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            if (userList != null)
                output = userList[0];

            return output;
        }
    }

}