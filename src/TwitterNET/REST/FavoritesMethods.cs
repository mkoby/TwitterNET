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
        /// <param name="statusRequestOptions">
        /// <see cref="StatusRequestOptions"/>
        /// </param>
        /// <returns>
        /// A <see cref="IList{T}"/>
        /// </returns>
        public IList<StatusMessage> GetFavorites(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/favorites.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnStatuses(responseText);
        }

        /// <summary>
        /// Favorites the specified status for the authenticated user 
        /// </summary>
        /// <param name="StatusID">
        /// ID of the status to favorite <see cref="System.Int64"/>
        /// </param>
        /// <returns>
        /// A <see cref="StatusMessage"/>
        /// </returns>
        public StatusMessage MarkAsFavorite(long StatusID)
        {
            string apiURL = "http://twitter.com/favorites/create/";
            string requestOptions = String.Format("{0}.xml", StatusID);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}", apiURL, requestOptions));
            requestOptions = null; //Clean up now un-needed objects
            IList<StatusMessage> statusMessages = ResponseParser.ReturnStatuses(responseText);

            return statusMessages[0];
        }

        /// <summary>
        /// Delete's a favorited status from the authenticated user's favorites 
        /// </summary>
        /// <param name="StatusID">
        /// ID of status to unfavorite <see cref="System.Int64"/>
        /// </param>
        /// <returns>
        /// Returns the favorite that was deleted <see cref="StatusMessage"/>
        /// </returns>
        public StatusMessage DeleteFavorite(long StatusID)
        {
            string apiURL = "http://twitter.com/favorites/destroy/";
            string requestOptions = String.Format("{0}.xml", StatusID);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}", apiURL, requestOptions));
            requestOptions = null; //Clean up now un-needed objects
            IList<StatusMessage> statusMessages = ResponseParser.ReturnStatuses(responseText);

            return statusMessages[0];
        }
    }
}