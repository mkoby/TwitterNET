using System.Collections.Generic;

namespace TwitterNET
{
    public partial class Twitter
    {
        public IList<long> GetFollowersList(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/followers/ids.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnUserIDs(responseText);
        }

        public IList<long> GetFollowingList(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/friends/ids.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnUserIDs(responseText);
        }
    }

}