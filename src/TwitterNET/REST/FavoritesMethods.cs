using System;
using System.Collections.Generic;

namespace TwitterNET
{
    public partial class Twitter
    {
        /// <summary>
        /// Gets the 20 most recent favorited statuses of a user
        /// Authenticated user by default. 
        /// </summary>
        /// <param name="statusRequestOptions">Accepted options: ID & Page</param>
        /// <returns>List of StatusMessage objects matching the supplied options</returns>
        public IList<StatusMessage> GetFavorites(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/favorites.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnStatuses(responseText);
        }

        /// <summary>
        /// Favorites the specified status for the authenticated user 
        /// </summary>
        /// <param name="StatusID">ID of the status to favorite</param>
        /// <returns>StatusMessage object representing the status being favorited</returns>
        public StatusMessage MarkAsFavorite(long StatusID)
        {
            StatusMessage output = null;
            string apiURL = "http://twitter.com/favorites/create/";
            string requestOptions = String.Format("{0}.xml", StatusID);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}", apiURL, requestOptions));
            requestOptions = null; //Clean up now un-needed objects
            IList<StatusMessage> statusMessages = ResponseParser.ReturnStatuses(responseText);

            if (statusMessages != null)
                output = statusMessages[0];

            return output;
        }

        /// <summary>
        /// Delete's a favorited status from the authenticated user's favorites
        /// </summary>
        /// <param name="StatusID">Status ID of the status to unfavorite</param>
        /// <returns>StatusMessage object representing the status that was unfavorited, NULL if fails</returns>
        public StatusMessage DeleteFavorite(long StatusID)
        {
            StatusMessage output = null;
            string apiURL = "http://twitter.com/favorites/destroy/";
            string requestOptions = String.Format("{0}.xml", StatusID);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}", apiURL, requestOptions));
            requestOptions = null; //Clean up now un-needed objects
            IList<StatusMessage> statusMessages = ResponseParser.ReturnStatuses(responseText);

            if(statusMessages != null)
                output = statusMessages[0];

            return output;
        }
    }
}