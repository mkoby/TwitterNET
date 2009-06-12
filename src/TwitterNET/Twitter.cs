using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace TwitterNET
{
    public class Twitter
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
		
		private StatusMessage ReturnSingleStatus(string responseText)
		{
			StatusMessage Output = null;
			
			if (!string.IsNullOrEmpty(responseText))
            {
                foreach(StatusMessage status in StatusMessage.Load(responseText))
				{
					Output = status;
					break; //we only want the first status (there should only be 1 anyway)
				}
            }
			
			return Output;
		}
		
		private IList<StatusMessage> ReturnListOfStatuses(string responseText)
		{
			IList<StatusMessage> Output = new List<StatusMessage>();
			
			if(!string.IsNullOrEmpty(responseText))
            {
                foreach (StatusMessage status in StatusMessage.Load(responseText))
					Output.Add(status);
            }
			
			return Output;
		}
		
		private IList<IUser> ReturnListOfUsers(string responseText)
		{
			IList<IUser> Output = new List<IUser>();
			
			if(!string.IsNullOrEmpty(responseText))
            {
                foreach (IUser user in User.Load(responseText))
					Output.Add(user);
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

        public void EndTwitterSession()
        {
            string apiURL = "http://twitter.com/account/end_session.xml";
            _requestHandler.MakeAPIRequest(_requestHandler, apiURL);
        }

        /// <summary>
        /// Returns the 20 most recent statuses from the public Twitter timeline
        /// </summary>
        /// <returns>List of StatusMessage objects</returns>
        public IList<StatusMessage> GetPublicTimeline()
        {
            string apiURL = "http://twitter.com/statuses/public_timeline.xml";
			string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);
			
            return ReturnListOfStatuses(responseText);
        }

        /// <summary>
        /// Returns a single twitter status
        /// </summary>
        /// <param name="StatusID"></param>
        /// <returns>The status ID of the status to request</returns>
        public StatusMessage GetSingleStatus(long StatusID)
        {
            if(StatusID <= 0)
                throw new ArgumentNullException("StatusID", 
				                                "StatusID can not be NULL or less than zero when requesting a single twitter status");

            string apiURL = "http://twitter.com/statuses/show/";
            string requestOptions = String.Format("{0}.xml", StatusID);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}", apiURL, requestOptions));
			requestOptions = null; //Clean up now un-needed objects

            return ReturnSingleStatus(responseText);
        }
		
		/// <summary>
		/// Deletes a single status owned by the authenticating user
		/// </summary>
		/// <param name="StatusID"></param>
		/// <returns></returns>
		public StatusMessage DeleteStatus(long StatusID)
		{
			if(StatusID <= 0)
                throw new ArgumentNullException("StatusID", 
				                                "StatusID can not be NULL or less than zero when requesting a single twitter status");
			
			if(!UserOwnsStatus(StatusID))
				throw new TwitterNetException("StatusUser cannot delete a status that is not their own");

			string apiURL = "http://twitter.com/statuses/destroy/";
            string requestOptions = String.Format("{0}.xml", StatusID);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}", apiURL, requestOptions));
			requestOptions = null; //Clean up now un-needed objects

            return ReturnSingleStatus(responseText);
		}

		/// <summary>
		/// Updates the authenticated user's status
		/// </summary>
		/// <param name="StatusText"></param>
		/// <returns></returns>
        public StatusMessage UpdateStatus(string StatusText)
        {
            return UpdateStatus(StatusText, long.MinValue);
        }

		/// <summary>
		/// Updates the authenticated user's status
		/// </summary>
		/// <param name="StatusText">The actual status message</param>
		/// <param name="InReplyStatusID">Status ID being replied to</param>
		/// <returns></returns>
        public StatusMessage UpdateStatus(string StatusText, long InReplyStatusID)
        {
            if (String.IsNullOrEmpty(StatusText))
                throw new ArgumentException("StatusText",
                                            "StatusText can not be NULL or EMPTY when updating twitter status");

            string apiUrl = "http://twitter.com/statuses/update.xml";
            StringBuilder requestOptions = new StringBuilder("?status=");
            string urlEncodedStatusText = HttpUtility.UrlEncode(StatusText);
            requestOptions.Append(urlEncodedStatusText);

            if (InReplyStatusID > long.MinValue)
                requestOptions.AppendFormat("&in_reply_to_status_id={0}", InReplyStatusID);

            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}", apiUrl, requestOptions));
            requestOptions = null; //Clean up now un-needed objects

            return ReturnSingleStatus(responseText);

        }

		/// <summary>
		/// Returns the authenticated user's friends timeline
		/// </summary>
		/// <param name="requestOptions"></param>
		/// <returns></returns>
		public IList<StatusMessage> GetFriendsTimeline(RequestOptions requestOptions)
		{
			string apiURL = "http://twitter.com/statuses/friends_timeline.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL));

            return ReturnListOfStatuses(responseText);
		}
		   
		/// <summary>
		/// Gets a specific user's timeline
		/// </summary>
		/// <param name="requestOptions"></param>
		/// <returns></returns>
		public IList<StatusMessage> GetUserTimeline(RequestOptions requestOptions)
		{
			string apiURL = "http://twitter.com/statuses/user_timeline.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL));

            return ReturnListOfStatuses(responseText);
		}
		
		/// <summary>
		/// Gets a user's friends and their most recent statuses 
		/// Authenticated user by default, use RequestOptions to be more specific.
		/// </summary>
		/// <param name="requestOptions">
		/// Accepts either the UserID or ScreenName and/or the Page RequestOptions <see cref="RequestOptions"/>
		/// </param>
		/// <returns>
		/// A list of users, with their most recent statuses <see cref="IList"/>
		/// </returns>
		public IList<IUser> GetUsersFriends(RequestOptions requestOptions)
        {
            string apiURL = "http://twitter.com/statuses/friends.xml";
            string resposneText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL));

            return ReturnListOfUsers(resposneText);
        }
		
		/// <summary>
		/// Gets a user's follower's and their most recent statuses. 
		/// Authenticated user by default, use RequestOptions to be more specific.
		/// </summary>
		/// <param name="requestOptions">
		/// Accepts either the UserID or ScreenName and/or the Page RequestOptions <see cref="RequestOptions"/>
		/// </param>
		/// <returns>
		/// A <see cref="IList"/>
		/// </returns>
		public IList<IUser> GetUsersFollowers(RequestOptions requestOptions)
		{
			string apiURL = "http://twitter.com/statuses/followers.xml";
			string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL));
			
			return ReturnListOfUsers(responseText);
		}

		/// <summary>
		/// Gets mentions of the authenticated user's screen name
		/// </summary>
		/// <param name="requestOptions"></param>
		/// <returns></returns>
        public IList<StatusMessage> GetMetions(RequestOptions requestOptions)
        {
            string apiURL = "http://twitter.com/statuses/mentions.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL));

            return ReturnListOfStatuses(responseText);
        }
		
		/// <summary>
		/// Gets the 20 most recent favorited statuses of a user
		/// Authenticated user by default. 
		/// </summary>
		/// <param name="requestOptions">
		/// <see cref="RequestOptions"/>
		/// </param>
		/// <returns>
		/// A <see cref="IList"/>
		/// </returns>
		public IList<StatusMessage> GetFavorites(RequestOptions requestOptions)
		{
			string apiURL = "http://twitter.com/favorites.xml";
			string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL));
			
			return ReturnListOfStatuses(responseText);
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
		public StatusMessage FavoriteStatus(long StatusID)
		{
			string apiURL = "http://twitter.com/favorites/create/";
			string requestOptions = String.Format("{0}.xml", StatusID);
			string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}",apiURL, requestOptions));
			requestOptions = null; //Clean up now un-needed objects

            return ReturnSingleStatus(responseText);
		}
		
		/// <summary>
		/// Delete's a favorited status from the authenticated user's favorites 
		/// </summary>
		/// <param name="StatusID">
		/// ID of status to unfavorite <see cref="System.Int64"/>
		/// </param>
		/// <returns>
		/// A <see cref="StatusMessage"/>
		/// </returns>
		public StatusMessage DeleteFavorite(long StatusID)
		{			
			string apiURL = "http://twitter.com/favorites/destroy/";
			string requestOptions = String.Format("{0}.xml", StatusID);
			string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}",apiURL, requestOptions));
			requestOptions = null; //Clean up now un-needed objects

            return ReturnSingleStatus(responseText);
		}
    }
}
