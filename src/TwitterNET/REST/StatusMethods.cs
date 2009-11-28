using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace TwitterNET
{
    public partial class Twitter
    {
        /// <summary>
        /// Returns a single twitter status
        /// </summary>
        /// <param name="StatusID">The status ID of the status to request</param>
        /// <returns>Single StatusMessage, matching the supplied StatusId or NULL if failed</returns>
        public StatusMessage GetSingleStatus(long StatusID)
        {
            if (StatusID <= 0)
                throw new ArgumentNullException("StatusID",
                                                "StatusID can not be NULL or less than zero when requesting a single twitter status");

            StatusMessage output = null;
            string apiURL = "http://twitter.com/statuses/show/";
            string requestOptions = String.Format("{0}.xml", StatusID);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}", apiURL, requestOptions));
            requestOptions = null; //Clean up now un-needed objects
            IList<StatusMessage> statusMessages = ResponseParser.ReturnStatuses(responseText);

            if (statusMessages != null)
                output = statusMessages[0];

            return output;
        }

        /// <summary>
        /// Deletes a single status owned by the authenticating user
        /// </summary>
        /// <param name="StatusID">The status ID of the status to delete</param>
        /// <returns>Single StatusMessage, matching the supplied StatusId or NULL if failed</returns>
        public StatusMessage DeleteStatus(long StatusID)
        {
            if (StatusID <= 0)
                throw new ArgumentNullException("StatusID",
                                                "StatusID can not be NULL or less than zero when requesting a single twitter status");

            if (!UserOwnsStatus(StatusID))
                throw new TwitterNetException("StatusUser cannot delete a status that is not their own");

            StatusMessage output = null;
            string apiURL = "http://twitter.com/statuses/destroy/";
            string requestOptions = String.Format("{0}.xml", StatusID);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}", apiURL, requestOptions));
            requestOptions = null; //Clean up now un-needed objects
            IList<StatusMessage> statusMessages = ResponseParser.ReturnStatuses(responseText);

            if (statusMessages != null)
                output = statusMessages[0];

            return output;
        }

        /// <summary>
        /// Updates the authenticated user's status
        /// </summary>
        /// <param name="StatusText">The text of the status update</param>
        /// <returns>Single StatusMessage object representing the newly created status</returns>
        public StatusMessage UpdateStatus(string StatusText)
        {
            return UpdateStatus(StatusText, long.MinValue);
        }

        /// <summary>
        /// Updates the authenticated user's status
        /// </summary>
        /// <param name="StatusText">The actual status message</param>
        /// <param name="InReplyStatusID">Status ID being replied to</param>
        /// <returns>Single StatusMessage object representing the newly created status</returns>
        public StatusMessage UpdateStatus(string StatusText, long InReplyStatusID)
        {
            if (String.IsNullOrEmpty(StatusText))
                throw new ArgumentException("StatusText", "StatusText can not be NULL or EMPTY when updating twitter status");

            StatusMessage output = null;
            string apiUrl = "http://twitter.com/statuses/update.xml";
            StringBuilder requestOptions = new StringBuilder("?status=");
            string urlEncodedStatusText = HttpUtility.UrlEncode(StatusText);
            requestOptions.Append(urlEncodedStatusText);

            if (InReplyStatusID > long.MinValue)
                requestOptions.AppendFormat("&in_reply_to_status_id={0}", InReplyStatusID);

            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, String.Format("{0}{1}", apiUrl, requestOptions));
            requestOptions = null; //Clean up now un-needed objects
            IList<StatusMessage> statusMessages = ResponseParser.ReturnStatuses(responseText);

            if (statusMessages != null)
                output = statusMessages[0];

            return output;
        }

        public RetweetedMessage RetweetStatus(long StatusId)
        {
            RetweetedMessage output = null;

            return output;
        }
    }
}