using System;
using System.Collections.Generic;

namespace TwitterNET
{
    public partial class Twitter
    {
        /// <summary>
        /// Checks to see if user1 follows the user2
        /// </summary>
        /// <param name="user1">Screen name of a user</param>
        /// <param name="user2">Screen name of user to check for friendship with user1</param>
        /// <returns>TRUE if user1 follows user2, otherwise false</returns>
        public bool CheckFriendship(string user1, string user2)
        {
            if (String.IsNullOrEmpty(user1) || String.IsNullOrEmpty(user2))
                throw new TwitterNetException(
                    "When checking if a friendship exists, both screen names must be not be NULL and not EMPTY");

            bool Output = false;

            //We're using the JSON API point for this call because it requires LESS post-processing
            string apiURL = "http://twitter.com/friendships/exists.json";
            string requestOptions = String.Format("?user_a={0}&user_b={1}", user1, user2);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}", apiURL, requestOptions));

            if (!String.IsNullOrEmpty(responseText))
                Output = bool.Parse(responseText) ? true : false;

            return Output;
        }

        /// <summary>
        /// Authenticated user to follow the supplied screen name
        /// </summary>
        /// <param name="screenName">Screen name of the user to follow</param>
        /// <param name="enableDeviceUpdates">Turn device notifications</param>
        /// <returns>User object representing the newly followed user or NULL if fails</returns>
        public IUser FollowUser(string screenName, bool enableDeviceUpdates)
        {
            IUser output = null;
            string apiURL = "http://twitter.com/friendships/create.xml";
            string requestOptions = String.Format("?screen_name={0}&follow={1}", screenName, enableDeviceUpdates);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                                                                 String.Format("{0}{1}", apiURL, requestOptions));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            if(userList != null)
                output = userList[0];

            return output;
        }

        /// <summary>
        /// Authenticated user will stop following supplied screen name
        /// </summary>
        /// <param name="screenName">Screen name of the user to unfollow</param>
        /// <returns>User object representing the unfollowed user or NULL if fails</returns>
        public IUser UnfollowUser(string screenName)
        {
            IUser output = null;
            string apiURL = "http://twitter.com/friendships/destroy.xml";
            string requestOptions = String.Format("?screen_name={0}", screenName);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                                                                 String.Format("{0}{1}", apiURL, requestOptions));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            if (userList != null)
                output = userList[0];

            return output;
        }
    }

}