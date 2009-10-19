using System.Collections.Generic;

namespace TwitterNET
{
    public partial class Twitter
    {
        /// <summary>
        /// Returns list of IDs of every user following the user specified in the supplied options
        /// </summary>
        /// <param name="statusRequestOptions">Accepted options: UserID & ScreenName</param>
        /// <returns>List of UserIDs</returns>
        public IList<long> GetFollowersList(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/followers/ids.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnUserIDs(responseText);
        }

        /// <summary>
        /// Returns list of IDs of every user being followed by the user specified in the supplied options
        /// </summary>
        /// <param name="statusRequestOptions">Accepted options: UserID & ScreenName</param>
        /// <returns>List of UserIDs</returns>
        public IList<long> GetFollowingList(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/friends/ids.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnUserIDs(responseText);
        }
    }

}