﻿using System;
using System.Collections.Generic;

namespace TwitterNET
{
    public partial class Twitter
    {
        /// <summary>
        /// Returns the 20 most recent statuses from the public Twitter timeline
        /// </summary>
        /// <returns>List of StatusMessage objects</returns>
        public IList<StatusMessage> GetPublicTimeline()
        {
            string apiURL = "http://twitter.com/statuses/public_timeline.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);

            return ResponseParser.ReturnStatuses(responseText);
        }

        /// <summary>
        /// Returns the authenticated user's friends timeline
        /// </summary>
        /// <param name="statusRequestOptions">Accepted options: SinceId, MaxId, Count, & Page</param>
        /// <returns></returns>
        public IList<StatusMessage> GetFriendsTimeline(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/statuses/friends_timeline.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnStatuses(responseText);
        }

        /// <summary>
        /// Gets a specific user's timeline
        /// </summary>
        /// <param name="statusRequestOptions">Accepted options: UserId, ScreenName, SinceId, MaxId, Count, & Page</param>
        /// <returns>List of StatusMessage objects of the specific user's statuses, matching the supplied StatusRequestOptions</returns>
        public IList<StatusMessage> GetUserTimeline(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/statuses/user_timeline.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnStatuses(responseText);
        }

        /// <summary>
        /// Gets mentions of the authenticated user's screen name
        /// </summary>
        /// <param name="statusRequestOptions">Accepted options: SinceId, MaxId, Count, & Page</param>
        /// <returns>List of StatusMessage objects, all @ mentions matching the supplied StatusRequestOptions</returns>
        public IList<StatusMessage> GetMetions(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/statuses/mentions.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnStatuses(responseText);
        }
    }
}