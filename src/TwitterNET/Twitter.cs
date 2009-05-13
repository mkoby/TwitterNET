using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace TwitterNET
{
    public class Twitter
    {
        readonly RequestHandler _requestHandler = null;

        public Twitter(string UserName, string Password)
        {
            _requestHandler = new RequestHandler(UserName, Password);
        }

        /// <summary>
        /// Returns the 20 most recent statuses from the public Twitter timeline
        /// </summary>
        /// <returns>List of IStatus objects</returns>
        public IList<IStatus> GetPublicTimeline()
        {
            IList<IStatus> Output = new List<IStatus>();
            string apiURL = "http://twitter.com/statuses/public_timeline.xml";
			string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL, String.Empty);
			
            if(!string.IsNullOrEmpty(responseText))
            {
                foreach (IStatus status in Status.ParseStatusArrayXml(responseText))
                {
                    Output.Add(status);
                }
            }

            return Output;
        }

        /// <summary>
        /// Returns a single twitter status
        /// </summary>
        /// <param name="StatusID"></param>
        /// <returns>The status ID of the status to request</returns>
        public IStatus GetSingleStatus(long StatusID)
        {
            if(StatusID <= 0)
                throw new ArgumentNullException("StatusID", "StatusID can not be NULL or less than zero when requesting a single twitter status");

            IStatus Output = null;
            string apiURL = "http://twitter.com/statuses/show/";
            string requestOptions = String.Format("{0}.xml", StatusID);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL, requestOptions);

            if (!string.IsNullOrEmpty(responseText))
            {
                Output = Status.ParseSingleStatusXml(responseText);
            }

            return Output;
        }
		
		/// <summary>
		/// Deletes a single status owned by the authenticating user
		/// </summary>
		/// <param name="StatusID"></param>
		/// <returns></returns>
		public IStatus DeleteStatus(long StatusID)
		{
			if(StatusID <= 0)
                throw new ArgumentNullException("StatusID", "StatusID can not be NULL or less than zero when requesting a single twitter status");

			IStatus statusToDestory = null, 
                    Output = null;
				
			try
			{
				statusToDestory = GetSingleStatus(StatusID);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
			if(statusToDestory != null)
			{
				if(statusToDestory.StatusUser.ScreenName.ToLowerInvariant() != _requestHandler.Login.ToLowerInvariant())
				{
					throw new TwitterNetException("StatusUser cannot delete a status that is not their own");
				}

	            string apiURL = "http://twitter.com/statuses/destroy/";
	            string requestOptions = String.Format("{0}.xml", StatusID);
	            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL, requestOptions);
	
	            if (!string.IsNullOrEmpty(responseText))
	            {
                    Output = Status.ParseSingleStatusXml(responseText);
	            }
			}

		    statusToDestory = null; //Clean up our objects
            return Output; //Return a NULL IStatus because we didn't get anything back
		}

		/// <summary>
		/// Updates the authenticated user's status
		/// </summary>
		/// <param name="StatusText"></param>
		/// <returns></returns>
        public IStatus UpdateStatus(string StatusText)
        {
            return UpdateStatus(StatusText, long.MinValue);
        }

		/// <summary>
		/// Updates the authenticated user's status
		/// </summary>
		/// <param name="StatusText">The actual status message</param>
		/// <param name="InReplyStatusID">Status ID being replied to</param>
		/// <returns></returns>
        public IStatus UpdateStatus(string StatusText, long InReplyStatusID)
        {
            if (String.IsNullOrEmpty(StatusText))
                throw new ArgumentException("StatusText",
                                            "StatusText can not be NULL or EMPTY when updating twitter status");

            IStatus Output = null;
            string apiUrl = "http://twitter.com/statuses/update.xml";
            StringBuilder requestOptions = new StringBuilder("?status=");
            string urlEncodedStatusText = HttpUtility.UrlEncode(StatusText);
            requestOptions.Append(urlEncodedStatusText);

            if (InReplyStatusID > long.MinValue)
                requestOptions.AppendFormat("&in_reply_to_status_id={0}", InReplyStatusID);

            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiUrl, requestOptions.ToString());

            if (!string.IsNullOrEmpty(responseText))
            {
                Output = Status.ParseSingleStatusXml(responseText);
            }

		    return Output;

        }

		/// <summary>
		/// Returns the authenticated user's friends timeline
		/// </summary>
		/// <param name="requestOptions"></param>
		/// <returns></returns>
		public IList<IStatus> GetFriendsTimeline(RequestOptions requestOptions)
		{
			IList<IStatus> Output = new List<IStatus>();
            string apiURL = "http://twitter.com/statuses/friends_timeline.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL), String.Empty);

            if (!string.IsNullOrEmpty(responseText))
            {
                foreach (IStatus status in Status.ParseStatusArrayXml(responseText))
                {
                    Output.Add(status);
                }
            }
			
			//Clean up our objects
			requestOptions = null;

            return Output;
		}
		   
		/// <summary>
		/// Gets a specific user's timeline
		/// </summary>
		/// <param name="requestOptions"></param>
		/// <returns></returns>
		public IList<IStatus> GetUserTimeline(RequestOptions requestOptions)
		{
			IList<IStatus> Output = new List<IStatus>();
            string apiURL = "http://twitter.com/statuses/user_timeline.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL), String.Empty);

            if (!string.IsNullOrEmpty(responseText))
            {
                foreach (IStatus status in Status.ParseStatusArrayXml(responseText))
                {
                    Output.Add(status);
                }
            }

			//Clean up our objects
			requestOptions = null;
			
            return Output;
		}

		/// <summary>
		/// Gets mentions of the authenticated user's screen name
		/// </summary>
		/// <param name="requestOptions"></param>
		/// <returns></returns>
        public IList<IStatus> GetMetions(RequestOptions requestOptions)
        {
            IList<IStatus> Output = new List<IStatus>();
            string apiURL = "http://twitter.com/statuses/mentions.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL), String.Empty);

            if (!string.IsNullOrEmpty(responseText))
            {
                foreach (IStatus status in Status.ParseStatusArrayXml(responseText))
                {
                    Output.Add(status);
                }
            }
			
            return Output;
        }

        public IList<IUser> GetUsersFriends(RequestOptions requestOptions)
        {
            IList<IUser> Output = new List<IUser>();
            string apiURL = "http://twitter.com/statuses/friends.xml";
            string resposneText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiURL), String.Empty);

            if(!String.IsNullOrEmpty(resposneText))
            {
                foreach(IUser user in User.ParseUserArrayXml(resposneText))
                {
                    Output.Add(user);
                }
            }
            
            return Output;
        }
    }
}
