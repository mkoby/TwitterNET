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
                //We have some XML to mess with
                Output = _requestHandler.RepsonseHandler(responseText);
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

            IList<IStatus> Output = null;
            string apiURL = "http://twitter.com/statuses/show/";
            string requestOptions = String.Format("{0}.xml", StatusID);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL, requestOptions);

            if (!string.IsNullOrEmpty(responseText))
            {
                //We have some XML to mess with
                Output = _requestHandler.RepsonseHandler(responseText);

                if (Output != null && Output.Count > 0)
                    return Output[0];
            }

            return null; //Return a NULL IStatus because we didn't get anything back
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

			IStatus statusToDestory = null;
				
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
				if(statusToDestory.User.ScreenName.ToLowerInvariant() != _requestHandler.Login.ToLowerInvariant())
				{
					throw new TwitterNetException("User cannot delete a status that is not their own");
				}
				
	            IList<IStatus> Output = null;
	            string apiURL = "http://twitter.com/statuses/destroy/";
	            string requestOptions = String.Format("{0}.xml", StatusID);
	            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL, requestOptions);
	
	            if (!string.IsNullOrEmpty(responseText))
	            {
	                //We have some XML to mess with
	                Output = _requestHandler.RepsonseHandler(responseText);
	
	                if (Output != null && Output.Count > 0)
	                    return Output[0];
	            }
			}

            return null; //Return a NULL IStatus because we didn't get anything back
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

            IList<IStatus> Output = null;
            string apiUrl = "http://twitter.com/statuses/update.xml";
            StringBuilder requestOptions = new StringBuilder("?status=");
            string urlEncodedStatusText = HttpUtility.UrlEncode(StatusText);
            requestOptions.Append(urlEncodedStatusText);

            if (InReplyStatusID > long.MinValue)
                requestOptions.AppendFormat("&in_reply_to_status_id={0}", InReplyStatusID);

            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiUrl, requestOptions.ToString());

            if (!string.IsNullOrEmpty(responseText))
            {
                //We have some XML to mess with
                Output = _requestHandler.RepsonseHandler(responseText);

                if (Output != null && Output.Count > 0)
                    return Output[0];
            }

            return null; //Return a NULL IStatus because we didn't get anything back

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
                //We have some XML to mess with
                Output = _requestHandler.RepsonseHandler(responseText);
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
                //We have some XML to mess with
                Output = _requestHandler.RepsonseHandler(responseText);
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
                //We have some XML to mess with
                Output = _requestHandler.RepsonseHandler(responseText);
            }

			//Clean up our objects
			requestOptions = null;
			
            return Output;
        }
    }
}
