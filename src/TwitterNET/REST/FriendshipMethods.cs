using System;
using System.Collections.Generic;

namespace TwitterNET
{
    public partial class Twitter
    {
        public bool CheckFriendship(string authenticatedUser, string checkUser)
        {
            if (String.IsNullOrEmpty(authenticatedUser) || String.IsNullOrEmpty(checkUser))
                throw new TwitterNetException(
                    "When checking if a friendship exists, both screen names must be not be NULL and not EMPTY");

            bool Output = false;

            //We're using the JSON API point for this call because it requires LESS post-processing
            string apiURL = "http://twitter.com/friendships/exists.json";
            string requestOptions = String.Format("?user_a={0}&user_b={1}", authenticatedUser, checkUser);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}", apiURL, requestOptions));

            if (!String.IsNullOrEmpty(responseText))
                Output = bool.Parse(responseText) ? true : false;

            return Output;
        }

        public IUser FollowUser(string screenName, bool enableDeviceUpdates)
        {
            string apiURL = "http://twitter.com/friendships/create.xml";
            string requestOptions = String.Format("?screen_name={0}&follow={1}", screenName, enableDeviceUpdates);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                                                                 String.Format("{0}{1}", apiURL, requestOptions));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            return userList[0];
        }

        public IUser UnfollowUser(string screenName)
        {
            string apiURL = "http://twitter.com/friendships/destroy.xml";
            string requestOptions = String.Format("?screen_name={0}", screenName);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                                                                 String.Format("{0}{1}", apiURL, requestOptions));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            return userList[0];
        }
    }

}