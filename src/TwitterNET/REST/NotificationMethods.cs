using System.Collections.Generic;

namespace TwitterNET
{
    public partial class Twitter
    {
        public IUser TurnDeviceNotificationsOn(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/notifications/follow.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            return userList[0];
        }

        public IUser TurnDeviceNotificationsOff(StatusRequestOptions statusRequestOptions)
        {
            string apiURL = "http://twitter.com/notifications/leave.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, statusRequestOptions.BuildRequestUri(apiURL));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            return userList[0];
        }
    }
}