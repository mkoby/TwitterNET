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
			
            return ResponseParser.ReturnListOfStatuses(responseText);
        }

        /// <summary>
        /// Returns a single twitter status
        /// </summary>
        /// <param name="StatusID">The status ID of the status to request</param>
        /// <returns></returns>
        public StatusMessage GetSingleStatus(long StatusID)
        {
            if(StatusID <= 0)
                throw new ArgumentNullException("StatusID", 
				                                "StatusID can not be NULL or less than zero when requesting a single twitter status");

            string apiURL = "http://twitter.com/statuses/show/";
            string requestOptions = String.Format("{0}.xml", StatusID);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}", apiURL, requestOptions));
			requestOptions = null; //Clean up now un-needed objects

            return ResponseParser.ReturnSingleStatus(responseText);
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

            return ResponseParser.ReturnSingleStatus(responseText);
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

            return ResponseParser.ReturnSingleStatus(responseText);

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

            return ResponseParser.ReturnListOfStatuses(responseText);
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

            return ResponseParser.ReturnListOfStatuses(responseText);
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

            return ResponseParser.ReturnListOfUsers(resposneText);
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
			
			return ResponseParser.ReturnListOfUsers(responseText);
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

            return ResponseParser.ReturnListOfStatuses(responseText);
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
			
			return ResponseParser.ReturnListOfStatuses(responseText);
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
			string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}",apiURL, requestOptions));
			requestOptions = null; //Clean up now un-needed objects

            return ResponseParser.ReturnSingleStatus(responseText);
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
			string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}",apiURL, requestOptions));
			requestOptions = null; //Clean up now un-needed objects

            return ResponseParser.ReturnSingleStatus(responseText);
		}

        public DirectMessage GetSingleDirectMessage(long id)
        {
            if (id < 0)
                throw new TwitterNetException("The ID must be a value greater than zero.");

            RequestOptions requestOptions = new RequestOptions();
            requestOptions.Add(RequestOptionNames.ID, id);

            return GetDirectMessages(requestOptions)[0];
        }

        public IList<DirectMessage> GetDirectMessages(RequestOptions requestOptions)
		{
			string apiURL = "http://twitter.com/direct_messages.xml";
			string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL));
			
			return ResponseParser.ReturnListofDirectMsgs(responseText);
		}

        public IList<DirectMessage> GetSentDirectMessages(RequestOptions requestOptions)
        {
            string apiURL = "http://twitter.com/direct_messages/sent.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnListofDirectMsgs(responseText);
        }

        public DirectMessage SendDirectMessage(string screenName, string messageText)
        {
            if (String.IsNullOrEmpty(screenName))
                throw new TwitterNetException(
                    "The screen name for the Direct Message recipient can not be empty or NULL");
            if (String.IsNullOrEmpty(messageText))
                throw new TwitterNetException(
                    "The message text for the Direct Message can not be empty or NULL");

            string apiURL = "http://twitter.com/direct_messages/new.xml";
            StringBuilder sb = new StringBuilder(apiURL);
            sb.AppendFormat("?screen_name={0}&text={1}", screenName, messageText);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, sb.ToString());
            sb = null; //Clean up un-needed objects

            return ResponseParser.ReternSingleDirectMsg(responseText);
        }

        public DirectMessage DeleteDirectMessage(long id)
        {
            if (id < 0)
                throw new TwitterNetException("The ID must be a value greater than zero.");

            //Check DM to ensure it's owned by the user
            if (!GetSingleDirectMessage(id).Recipient.ScreenName.ToLower().Equals(_requestHandler.Login.ToLower()))
                throw new TwitterNetException(
                    "Authenticated user must be the recipient of the Direct Message being deleted");


            string apiURL = "http://twitter.com/direct_messages/destroy/";
            StringBuilder sb = new StringBuilder(apiURL);
            sb.AppendFormat("{0}.xml", id);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, sb.ToString());
            sb = null; //Clean up un-needed objects

            return ResponseParser.ReternSingleDirectMsg(responseText);
        }

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
            Output = (String.IsNullOrEmpty(responseText)) ? bool.Parse(responseText) : false;

            return Output;
        }

        public IUser FollowUser(string screenName, bool enableDeviceUpdates)
        {
            string apiURL = "http://twitter.com/friendships/create.xml";
            string requestOptions = String.Format("?screen_name={0}&follow={1}", screenName, enableDeviceUpdates);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                                                                 String.Format("{0}{1}", apiURL, requestOptions));

            return ResponseParser.ReturnSingleUser(responseText);
        }

        public IUser UnfollowUser(string screenName)
        {
            string apiURL = "http://twitter.com/friendships/destroy.xml";
            string requestOptions = String.Format("?screen_name={0}", screenName);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                                                                 String.Format("{0}{1}", apiURL, requestOptions));

            return ResponseParser.ReturnSingleUser(responseText);
        }

        public IList<long> GetFollowersList(RequestOptions requestOptions)
        {
            string apiURL = "http://twitter.com/followers/ids.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnListOfUserIDs(responseText);
        }

        public IList<long> GetFollowingList(RequestOptions requestOptions)
        {
            string apiURL = "http://twitter.com/friends/ids.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnListOfUserIDs(responseText);
        }

        public IUser TurnDeviceNotificationsOn(RequestOptions requestOptions)
        {
            string apiURL = "http://twitter.com/notifications/follow.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnSingleUser(responseText);
        }

        public IUser TurnDeviceNotificationsOff(RequestOptions requestOptions)
        {
            string apiURL = "http://twitter.com/notifications/leave.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnSingleUser(responseText);
        }

        public IUser VerifyCredentials(string userName, string password)
        {
            string apiURL = "http://twitter.com/account/verify_credentials.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);

            return ResponseParser.ReturnSingleUser(responseText);
        }
    }
}
