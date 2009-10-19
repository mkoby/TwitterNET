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

        /// <summary>
        /// Verifies the supplied credentials
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>User of the supplied credentials or NULL if not verified</returns>
        public IUser VerifyCredentials(string userName, string password)
        {
            IUser output = null;
            string apiURL = "http://twitter.com/account/verify_credentials.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            if(userList != null)
                output = userList[0];

            return output;
        }

        /// <summary>
        /// Ends an authenticated session
        /// </summary>
        /// <returns>Dicitionary of hash returned from the API</returns>
        public IDictionary<string, object> EndTwitterSession()
        {
            string apiURL = "http://twitter.com/account/end_session.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);

            return ResponseParser.ReturnHashDictionary(responseText);
        }

        /// <summary>
        /// Checks the rate limits for the authenticated user, if not authenticated then for the IP address
        /// </summary>
        /// <returns>Dictionary of hash returned from the API</returns>
        public IDictionary<string, object> RateLimitStatus()
        {
            string apiURL = "http://twitter.com/account/rate_limit_status.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);

            return ResponseParser.ReturnHashDictionary(responseText);
        }

        /// <summary>
        /// Sets which device Twitter delivers updates to for the authenticating user.  
        /// Sending None as the device parameter will disable IM or SMS updates.
        /// </summary>
        /// <param name="deviceType"></param>
        /// <returns>User object representing the authenticated user</returns>
        public IUser UpdateDeliveryDevice(DeliveryDeviceType deviceType)
        {
            IUser output = null;
            string apiURL = "http://twitter.com/account/update_delivery_device.xml";
            string requestOptions = String.Format("?device={0}", deviceType.ToString().ToLower());
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                                                                 String.Format("{0}{1}", apiURL, requestOptions));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            if (userList != null)
                output = userList[0];

            return output;
        }

        /// <summary>
        /// Updates the profile colors for the authenticated user's profile
        /// </summary>
        /// <param name="requestOptions">Accepted options: ProfileBackgroundColor, ProfileTextColor, ProfileLinkColor, ProfileSidebarFillColor, & ProfileSidebarBorderColor </param>
        /// <returns>User object representing the authenticated user containing the updated profile colors</returns>
        public IUser UpdateProfileColors(AccountRequestOptions requestOptions)
        {
            IUser output = null;
            string apiUrl = "http://twitter.com/account/update_profile_colors.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiUrl));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            if (userList != null)
                output = userList[0];

            return output;
        }

        /// <summary>
        /// Updates the authenticated user's avatar
        /// </summary>
        /// <param name="imagefile">Image file to upload</param>
        /// <returns>User object representing the authenticated user with the updated avatar file location</returns>
        public IUser UpdateProfileImage(string imagefile)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the authenticated user's profile background image
        /// </summary>
        /// <param name="imagefile">Image file to upload</param>
        /// <returns>User object representing the authenticated user with the updated profile background information</returns>
        public IUser UpdateProfileBackgroundImage(string imagefile)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the authenticated user's profile information
        /// </summary>
        /// <param name="requestOptions">Accpeted options: Name, Website, Location, & Description</param>
        /// <returns>User object representing the authenticated user with the updaed information</returns>
        public IUser UpdateProfileInfo(AccountRequestOptions requestOptions)
        {
            IUser output = null;
            string apiUrl = "http://twitter.com/account/update_profile.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, requestOptions.BuildRequestUri(apiUrl));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            if (userList != null)
                output = userList[0];

            return output;
        }
    }

}