using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TwitterNET
{
    public partial class Twitter
    {
        readonly RequestHandler _requestHandler = null;
		
		private bool UserOwnsStatus(long StatusID)
		{
			bool Output = true;
			StatusMessage statusToDestory = null;
			
			try
			{
				statusToDestory = GetSingleStatus(StatusID);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
			if(statusToDestory != null && 
			   statusToDestory.Author.ScreenName.ToLowerInvariant() != _requestHandler.Login.ToLowerInvariant())
			{
				Output = false;
			}
			
			return Output;
		}

		/// <summary>
		/// Create a Twitter object with no associated login 
		/// </summary>
		public Twitter()
		{
			_requestHandler = new RequestHandler(String.Empty, String.Empty);
		}
		
		/// <summary>
		/// Creates a new Twitter object with an associated login 
		/// </summary>
		/// <param name="UserName">
		/// The username of the user
		/// </param>
		/// <param name="Password">
		/// The password of the user
		/// </param>
        public Twitter(string UserName, string Password)
        {
            _requestHandler = new RequestHandler(UserName, Password);
        }

        /// <summary>
        /// Returns a single twitter user object
        /// </summary>
        /// <param name="screenName">The screen name of the user to request</param>
        /// <returns></returns>
        public IUser GetSingleUser(string screenName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a single twitter user object
        /// </summary>
        /// <param name="userID">The ID (numeric) of the user to request</param>
        /// <returns></returns>
        public IUser GetSingleUser(long userID)
        {
            throw new NotImplementedException();
        }

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
