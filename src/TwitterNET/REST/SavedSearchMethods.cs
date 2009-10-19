using System;
using System.Collections.Generic;
using System.Web;

namespace TwitterNET
{
    public partial class Twitter
    {
        /// <summary>
        /// Creates a saved search using the specified query
        /// </summary>
        /// <param name="searchQuery">Search terms</param>
        /// <returns>SavedSearch object representing the created saved search or NULL if fails</returns>
        public SavedSearch CreateSavedSearch(string searchQuery)
        {
            SavedSearch output = null;
            string apiURL = "http://twitter.com/saved_searches/create.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                        String.Format("{0}?query={1}", apiURL, HttpUtility.UrlEncode(searchQuery)));
            IList<SavedSearch> savedSearches = ResponseParser.ReturnSavedSearches(responseText);

            if (savedSearches != null)
                output = savedSearches[0];

            return output;
        }

        /// <summary>
        /// Deletes the specified saved search
        /// </summary>
        /// <param name="id">ID of the saved search to delete</param>
        /// <returns>SavedSearch object representing the deleted saved search or NULL if fails</returns>
        public SavedSearch DeleteSavedSearch(long id)
        {
            SavedSearch output = null;
            string apiURL = "http://twitter.com/saved_searches/destroy/";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}.xml", apiURL, id));
            IList<SavedSearch> savedSearches = ResponseParser.ReturnSavedSearches(responseText);

            if (savedSearches != null)
                output = savedSearches[0];

            return output;
        }

        /// <summary>
        /// Gets a list of the authenticated user's Saved Searches
        /// </summary>
        /// <returns>List of SavedSearch objects representing the saved searches created by the authenticated user</returns>
        public IList<SavedSearch> GetSavedSearches()
        {
            string apiURL = "http://twitter.com/saved_searches.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);

            return ResponseParser.ReturnSavedSearches(responseText);
        }

        /// <summary>
        /// Returns the saved search matching the ID
        /// </summary>
        /// <param name="id">ID of saved search to retrieve</param>
        /// <returns>SavedSearch object representing the requested saved search or NULL if fails</returns>
        public SavedSearch ShowSavedSearch(long id)
        {
            SavedSearch output = null;
            string apiURL = "http://twitter.com/saved_searches/show/";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}.xml", apiURL, id));
            IList<SavedSearch> savedSearches = ResponseParser.ReturnSavedSearches(responseText);

            if (savedSearches != null)
                output = savedSearches[0];

            return output;
        }
    }

}