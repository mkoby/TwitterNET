using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterNET
{
    public partial class Twitter
    {
        public DirectMessage GetSingleDirectMessage(long id)
        {
            if (id < 0)
                throw new TwitterNetException("The ID must be a value greater than zero.");

            StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
            statusRequestOptions.Add(StatusRequestOptionNames.ID, id);

            return GetDirectMessages(statusRequestOptions)[0];
        }

        public IList<DirectMessage> GetDirectMessages(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/direct_messages.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnDirectMsgs(responseText);
        }

        public IList<DirectMessage> GetSentDirectMessages(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/direct_messages/sent.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));

            return ResponseParser.ReturnDirectMsgs(responseText);
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
            IList<DirectMessage> dmList = ResponseParser.ReturnDirectMsgs(responseText);

            return dmList[0];
        }

        public DirectMessage DeleteDirectMessage(long id)
        {
            if (id < 0)
                throw new TwitterNetException("The ID must be a value greater than zero.");

            //Check DM to ensure it's owned by the user
            DirectMessage dmTest = GetSingleDirectMessage(id);
            if (!dmTest.Recipient.ScreenName.ToLower().Equals(_requestHandler.Login.ToLower()))
                throw new TwitterNetException(
                    "Authenticated user must be the recipient of the Direct Message being deleted");


            string apiURL = "http://twitter.com/direct_messages/destroy/";
            StringBuilder sb = new StringBuilder(apiURL);
            sb.AppendFormat("{0}.xml", id);
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, sb.ToString());
            sb = null; //Clean up un-needed objects
            IList<DirectMessage> dmList = ResponseParser.ReturnDirectMsgs(responseText);

            return dmList[0];
        }
    }

}