using System;
using System.Collections.Generic;

namespace TwitterNET
{
    public partial class Twitter
    {
        public enum DeliveryDeviceType
        {
            IM,
            SMS,
            None
        }

        public IUser VerifyCredentials(string userName, string password)
        {
            string apiURL = "http://twitter.com/account/verify_credentials.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);

            return ResponseParser.ReturnSingleUser(responseText);
        }

        public IDictionary<string, object> EndTwitterSession()
        {
            string apiURL = "http://twitter.com/account/end_session.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);

            return ResponseParser.ReturnHashDictionary(responseText);
        }

        public IDictionary<string, object> RateLimitStatus()
        {
            string apiURL = "http://twitter.com/account/rate_limit_status.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);

            return ResponseParser.ReturnHashDictionary(responseText);
        }

        public IUser UpdateDeliveryDevice(DeliveryDeviceType deviceType)
        {
            string apiURL = "http://twitter.com/account/update_delivery_device.xml";
            string requestOptions = String.Format("?device={0}", deviceType.ToString().ToLower());
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                                                                 String.Format("{0}{1}", apiURL, requestOptions));

            return ResponseParser.ReturnSingleUser(responseText);
        }

        public IUser UpdateProfileColors(AccountRequestOptions requestOptions)
        {
            string apiUrl = "http://twitter.com/account/update_profile_colors.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiUrl));

            return ResponseParser.ReturnSingleUser(responseText);
        }

        public IUser UpdateProfileImage(string imagefile)
        {
            throw new NotImplementedException();
        }

        public IUser UpdateProfileBackgroundImage(string imagefile)
        {
            throw new NotImplementedException();
        }

        public IUser UpdateProfileInfo(AccountRequestOptions requestOptions)
        {
            string apiUrl = "http://twitter.com/account/update_profile.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiUrl));

            return ResponseParser.ReturnSingleUser(responseText);
        }
    }

}