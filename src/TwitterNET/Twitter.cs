using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Web;

namespace TwitterNET
{
    public class Twitter
    {
        readonly RequestHandler requestHandler = null;

        public Twitter(string UserName, string Password)
        {
            requestHandler = new RequestHandler(UserName, Password);
        }

        /// <summary>
        /// Returns the 20 most recent statuses from the public Twitter timeline
        /// </summary>
        /// <returns>List of IStatus objects</returns>
        public IList<IStatus> GetPublicTimeline()
        {
            IList<IStatus> Output = new List<IStatus>();
            string apiURL = "http://twitter.com/statuses/public_timeline.xml";
			string responseText = requestHandler.MakeAPIRequest(requestHandler, apiURL, String.Empty);
			
            if(!string.IsNullOrEmpty(responseText))
            {
                //We have some XML to mess with
                Output = requestHandler.RepsonseHandler(responseText);
            }

            return Output;
        }

        public IStatus GetSingleStatus(long StatusID)
        {
            if(StatusID <= 0)
                throw new ArgumentNullException("StatusID", "StatusID can not be NULL or less than zero when requesting a single twitter status");

            IList<IStatus> Output = null;
            string apiURL = "http://twitter.com/statuses/show/";
            string requestOptions = String.Format("{0}.xml", StatusID);
            string responseText = requestHandler.MakeAPIRequest(requestHandler, apiURL, requestOptions);

            if (!string.IsNullOrEmpty(responseText))
            {
                //We have some XML to mess with
                Output = requestHandler.RepsonseHandler(responseText);

                if (Output != null && Output.Count > 0)
                    return Output[0];
            }

            return null; //Return a NULL IStatus because we didn't get anything back
        }

        public IStatus UpdateStatus(string StatusText)
        {
            return UpdateStatus(StatusText, long.MinValue);
        }

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

            string responseText = requestHandler.MakeAPIRequest(requestHandler, apiUrl, requestOptions.ToString());

            if (!string.IsNullOrEmpty(responseText))
            {
                //We have some XML to mess with
                Output = requestHandler.RepsonseHandler(responseText);

                if (Output != null && Output.Count > 0)
                    return Output[0];
            }

            return null; //Return a NULL IStatus because we didn't get anything back

        }

        /*******************************************************
         * BEGIN Friends Timline Calls
         * *****************************************************/

        private IList<IStatus> GetFriendsTimeline(DateTime SinceStatusDate, long SinceStatusID, long MaxStatusID, int ReturnCount, int PageNumber)
        {
            StringBuilder requestOptions = new StringBuilder(String.Empty);

            #region Parse Options

            if (SinceStatusDate != DateTime.MinValue)
            {
                if (requestOptions.Length == 0)
                    requestOptions.AppendFormat("?since={0}", SinceStatusDate.ToString("ddd MMM dd HH:mm:ss zzz yyyy"));
                else
                    requestOptions.AppendFormat("&since={0}", SinceStatusDate.ToString("ddd MMM dd HH:mm:ss zzz yyyy"));
            }

            if (SinceStatusID != long.MinValue)
            {
                if (requestOptions.Length == 0)
                    requestOptions.AppendFormat("?since_id={0}", SinceStatusID);
                else
                    requestOptions.AppendFormat("&since_id={0}", SinceStatusID);
            }

            if (MaxStatusID != long.MinValue)
            {
                if (requestOptions.Length == 0)
                    requestOptions.AppendFormat("?max_id={0}", MaxStatusID);
                else
                    requestOptions.AppendFormat("&max_id={0}", MaxStatusID);
            }

            if (ReturnCount != int.MinValue)
            {
                if (requestOptions.Length == 0)
                    requestOptions.AppendFormat("?count={0}", ReturnCount);
                else
                    requestOptions.AppendFormat("&count={0}", ReturnCount);
            }

            if (PageNumber != int.MinValue)
            {
                if (requestOptions.Length == 0)
                    requestOptions.AppendFormat("?page={0}", PageNumber);
                else
                    requestOptions.AppendFormat("&page={0}", PageNumber);
            }

            #endregion

            IList<IStatus> Output = new List<IStatus>();
            string apiURL = "http://twitter.com/statuses/friends_timeline.xml";
            string responseText = requestHandler.MakeAPIRequest(requestHandler, apiURL, requestOptions.ToString());

            if (!string.IsNullOrEmpty(responseText))
            {
                //We have some XML to mess with
                Output = requestHandler.RepsonseHandler(responseText);
            }

            return Output;
        }

        /// <summary>
        /// Returns the authenticated user's friend's statuses with no options passed to the Twitter API
        /// </summary>
        /// <returns>List of IStatus objects</returns>
        public IList<IStatus> GetFriendsTimeline()
        {
            return GetFriendsTimeline(DateTime.MinValue, long.MinValue, long.MinValue, int.MinValue, int.MinValue);
        }

        /// <summary>
        /// Returns the authenticated user's friend's statuses since the date passed
        /// </summary>
        /// <param name="SinceStatusDate">DateTime of earliest update</param>
        /// <param name="ReturnCount">Number of statuses to return</param>
        /// <returns>List of IStatus objects</returns>
        public IList<IStatus> GetFriendsTimeline(DateTime SinceStatusDate, int ReturnCount)
        {
            return GetFriendsTimeline(SinceStatusDate, long.MinValue, long.MinValue, ReturnCount, int.MinValue);
        }


        /// <summary>
        /// Returns the authenticated user's friend's statuses since the StatusID passed.  
        /// The StatusID can either be the StatusID we want all records after or the maximum StatusID we want all records prior to.
        /// </summary>
        /// <param name="StatusID">Status ID we want to pass to the Twitter API</param>
        /// <param name="IsMaxStatus">Set to TRUE if you are looking to get all previous status records from the ID passed</param>
        /// <param name="ReturnCount">Number of statuses to return</param>
        /// <returns>List of IStatus objects</returns>
        public IList<IStatus> GetFriendsTimeline(long StatusID, bool IsMaxStatus, int ReturnCount)
        {
            if(IsMaxStatus)
                return GetFriendsTimeline(DateTime.MinValue, long.MinValue, StatusID, ReturnCount, int.MinValue);
                
            
            //We're passing for SINCE StatusID rather than MAX StatusID
            return GetFriendsTimeline(DateTime.MinValue, StatusID, long.MinValue, ReturnCount, int.MinValue);
        }

        /// <summary>
        /// Returns the authenticated user's friend's statuses from a specific "page"
        /// This is like on the Twitter website when you hit the "Older" or "More" links/buttons
        /// </summary>
        /// <param name="PageNumber">The page number you want to pull statuses from</param>
        /// <param name="ReturnCount">Number of statuses to return</param>
        /// <returns></returns>
        public IList<IStatus> GetFriendsTimeline(int PageNumber, int ReturnCount)
        {
            return GetFriendsTimeline(DateTime.MinValue, long.MinValue, long.MinValue, ReturnCount, PageNumber);
        }


        /*******************************************************
         * BEGIN User Timline Calls
         * *****************************************************/

        private IList<IStatus> GetUserTimeline(string UserName, long UserID, long SinceStatusID, long MaxStatusID, int PageNumber )
        {
            StringBuilder requestOptions = new StringBuilder(String.Empty);

            #region Parse Arguments

            if(!String.IsNullOrEmpty(UserName) || UserID > long.MinValue)
            {
                string requestValue = String.Empty;
                string urlProperty = "screen_name";
                //bool withStatusID = (SinceStatusID == long.MinValue && MaxStatusID == long.MinValue ? false : true);

                if(!String.IsNullOrEmpty(UserName) && UserID == long.MinValue)
                {
                    requestValue = UserName;
                }
                else if(String.IsNullOrEmpty(UserName) && UserID > long.MinValue)
                {
                    requestValue = UserID.ToString();
                    urlProperty = "user_id";
                }

                if (requestOptions.Length == 0)
                    requestOptions.AppendFormat("?{0}={1}", urlProperty, requestValue);
                else
                    requestOptions.AppendFormat("&{0}={1}", urlProperty, requestValue);
            }

            if(SinceStatusID > long.MinValue || MaxStatusID > long.MinValue)
            {
                long requestValue = long.MinValue;
                string urlProperty = "since_id";

                if (SinceStatusID > long.MinValue)
                    requestValue = SinceStatusID;
                else if (MaxStatusID > long.MinValue)
                {
                    requestValue = MaxStatusID;
                    urlProperty = "max_id";
                }

                if (requestOptions.Length == 0)
                    requestOptions.AppendFormat("?{0}={1}", urlProperty, requestValue);
                else
                    requestOptions.AppendFormat("&{0}={1}", urlProperty, requestValue);
            }


            if (PageNumber > int.MinValue)
            {
                if (requestOptions.Length == 0)
                    requestOptions.AppendFormat("?page={0}", PageNumber);
                else
                    requestOptions.AppendFormat("&page={0}", PageNumber);
            }
            
            #endregion Parse Arguments
            
            IList<IStatus> Output = new List<IStatus>();
            string apiURL = "http://twitter.com/statuses/user_timeline.xml";
            string responseText = requestHandler.MakeAPIRequest(requestHandler, apiURL, requestOptions.ToString());

            if (!string.IsNullOrEmpty(responseText))
            {
                //We have some XML to mess with
                Output = requestHandler.RepsonseHandler(responseText);
            }

            return Output;
        }

        public IList<IStatus> GetUserTimeline(string UserName)
        {
            return GetUserTimeline(UserName, long.MinValue, long.MinValue, long.MinValue, int.MinValue);
        }
        
        public IList<IStatus> GetUserTimeline(string UserName, int PageNumber)
        {
            return GetUserTimeline(UserName, long.MinValue, long.MinValue, long.MinValue, PageNumber);
        }

        public IList<IStatus> GetUserTimeline(long UserID)
        {
            return GetUserTimeline(string.Empty, UserID, long.MinValue, long.MinValue, int.MinValue);
        }

        public IList<IStatus> GetUserTimeline(long UserID, int PageNumber)
        {
            return GetUserTimeline(string.Empty, UserID, long.MinValue, long.MinValue, PageNumber);
        }

        public IList<IStatus> GetUserTimeline(string UserName, long StatusID, bool IsMaxID)
        {
            return IsMaxID ? GetUserTimeline(UserName, long.MinValue, long.MinValue, StatusID, int.MinValue)
                : GetUserTimeline(UserName, long.MinValue, StatusID, long.MinValue, int.MinValue);
        }

        /*******************************************************
         * BEGIN User Timline Calls
         * *****************************************************/

        public IList<IStatus> GetMetions()
        {
            return GetMetions(int.MinValue);
        }

        public IList<IStatus> GetMetions(int count)
        {
            return GetMetions(int.MinValue, count);
        }

        public IList<IStatus> GetMetions(int pageNumber, int count)
        {
            StringBuilder requestOptions = new StringBuilder();

            if (pageNumber > 0)
            {
                if (requestOptions.Length == 0)
                    requestOptions.AppendFormat("?page={0}", pageNumber);
                else
                    requestOptions.AppendFormat("&page={0}", pageNumber);
            }

            if (count > 0)
            {
                if (requestOptions.Length == 0)
                    requestOptions.AppendFormat("?count={0}", count);
                else
                    requestOptions.AppendFormat("&count={0}", count);
            }


            IList<IStatus> Output = new List<IStatus>();
            string apiURL = "http://twitter.com/statuses/mentions.xml";
            string responseText = requestHandler.MakeAPIRequest(requestHandler, apiURL, requestOptions.ToString());

            if (!string.IsNullOrEmpty(responseText))
            {
                //We have some XML to mess with
                Output = requestHandler.RepsonseHandler(responseText);
            }

            return Output;
        }
    }
}
