using System.Collections.Generic;

namespace TwitterNET
{
    public partial class Twitter
    {
        /// <summary>
        /// Turn device notifications on for user specified in the options
        /// </summary>
        /// <param name="statusRequestOptions">Accepted options: UserID & ScreenName</param>
        /// <returns>User object representing the user for whom device notifications were turned on</returns>
        public IUser TurnDeviceNotificationsOn(StatusRequestOptions statusRequestOptions)
        {
            IUser output = null;
            string apiURL = "http://twitter.com/notifications/follow.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            if (userList != null)
                output = userList[0];

            return output;
        }

        /// <summary>
        /// Turn notifications off for the user specified in the options
        /// </summary>
        /// <param name="statusRequestOptions">Accepted options: UserID & ScreenName</param>
        /// <returns>User object representing the user for whom device notifications were turned off</returns>
        public IUser TurnDeviceNotificationsOff(StatusRequestOptions statusRequestOptions)
        {
            IUser output = null;
            string apiURL = "http://twitter.com/notifications/leave.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            if (userList != null)
                output = userList[0];

            return output;
        }
    }
}