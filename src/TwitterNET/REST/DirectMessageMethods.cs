using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterNET
{
    public partial class Twitter
    {
        /// <summary>
        /// Gets a single direct message, matching the supplied id
        /// </summary>
        /// <param name="id">The id of the direct message to retrieve</param>
        /// <returns>Single DirectMessage object representing the requested direct message</returns>
        public DirectMessage GetSingleDirectMessage(long id)
        {
            if (id < 0)
                throw new TwitterNetException("The ID must be a value greater than zero.");

            DirectMessage output = null;
            StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
            statusRequestOptions.Add(StatusRequestOptionNames.ID, id);
            IList<DirectMessage> dmList = GetDirectMessages(statusRequestOptions);

            if (dmList != null)
                output = dmList[0];

            return output;
        }

        /// <summary>
        /// Returns list of direct messages matching the supplied options
        /// </summary>
        /// <param name="statusRequestOptions">Accepted options: SinceID, MaxID, Count, & Page</param>
        /// <returns>List of DirectMessage objects matching the supplied options</returns>
        public IList<DirectMessage> GetDirectMessages(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/direct_messages.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnDirectMsgs(responseText);
        }

        /// <summary>
        /// Returns a list of direct messages sent by the autenthicated user, matching the supplied options
        /// </summary>
        /// <param name="statusRequestOptions">Accepted options: SinceID, MaxID, Count, & Page</param>
        /// <returns>List of DirectMessage objects sent by the authenticated user, matching the supplied options</returns>
        public IList<DirectMessage> GetSentDirectMessages(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/direct_messages/sent.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnDirectMsgs(responseText);
        }

        /// <summary>
        /// Sends a new direct message to the supplied user
        /// </summary>
        /// <param name="screenName">Screen name of the user to send the direct message to</param>
        /// <param name="messageText">Text of the message being sent</param>
        /// <returns>DirectMessage object of the sent message or NULL if failed</returns>
        public DirectMessage SendDirectMessage(string screenName, string messageText)
        {
            if (String.IsNullOrEmpty(screenName))
                throw new TwitterNetException(
                    "The screen name for the Direct Message recipient can not be empty or NULL");
            if (String.IsNullOrEmpty(messageText))
                throw new TwitterNetException(
                    "The message text for the Direct Message can not be empty or NULL");

            DirectMessage output = null;
            string apiURL = "http://twitter.com/direct_messages/new.xml";
            StringBuilder sb = new StringBuilder(apiURL);
            sb.AppendFormat("?screen_name={0}&text={1}", screenName, messageText);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, sb.ToString());
            sb = null; //Clean up un-needed objects
            IList<DirectMessage> dmList = ResponseParser.ReturnDirectMsgs(responseText);

            if(dmList != null)
                output = dmList[0];

            return output;
        }

        /// <summary>
        /// Deletes the direct message matching the supplied ID
        /// </summary>
        /// <param name="id">The message ID of the direct message to delete</param>
        /// <returns>DirectMessage object representing the deleted message or NULL if failed</returns>
        public DirectMessage DeleteDirectMessage(long id)
        {
            if (id < 0)
                throw new TwitterNetException("The ID must be a value greater than zero.");

            //Check DM to ensure it's owned by the user
            DirectMessage dmTest = GetSingleDirectMessage(id);
            if (!dmTest.Recipient.ScreenName.ToLower().Equals(_requestHandler.Login.ToLower()))
                throw new TwitterNetException(
                    "Authenticated user must be the recipient of the Direct Message being deleted");

            DirectMessage output = null;
            string apiURL = "http://twitter.com/direct_messages/destroy/";
            StringBuilder sb = new StringBuilder(apiURL);
            sb.AppendFormat("{0}.xml", id);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, sb.ToString());
            sb = null; //Clean up un-needed objects
            IList<DirectMessage> dmList = ResponseParser.ReturnDirectMsgs(responseText);

            if (dmList != null)
                output = dmList[0];

            return output;
        }
    }

}