using System;
using System.Collections.Generic;
using System.Web;

namespace TwitterNET
{
    public partial class Twitter
    {
        public SavedSearch CreateSavedSearch(string searchQuery)
        {
            string apiURL = "http://twitter.com/saved_searches/create.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                        String.Format("{0}?query={1}", apiURL, HttpUtility.UrlEncode(searchQuery)));

            return ResponseParser.ReturnSingleSavedSearch(responseText);
        }

        public SavedSearch DeleteSavedSearch(long id)
        {
            string apiURL = "http://twitter.com/saved_searches/destroy/";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}.xml", apiURL, id));

            return ResponseParser.ReturnSingleSavedSearch(responseText);
        }

        public IList<SavedSearch> GetSavedSearches()
        {
            string apiURL = "http://twitter.com/saved_searches.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);

            return ResponseParser.ReturnListOfSavedSearches(responseText);
        }

        public SavedSearch ShowSavedSearch(long id)
        {
            string apiURL = "http://twitter.com/saved_searches/show/";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}.xml", apiURL, id));

            return ResponseParser.ReturnSingleSavedSearch(responseText);
        }
    }

}