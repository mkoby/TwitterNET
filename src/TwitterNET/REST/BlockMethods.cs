using System;
using System.Collections.Generic;

namespace TwitterNET
{
    public partial class Twitter
    {
        /// <summary>
        /// Blocks the user matching the supplied userId
        /// </summary>
        /// <param name="userId">ID of the user to block</param>
        /// <returns>User object representing the user blocked</returns>
        public IUser BlockUser(long userId)
        {
            IUser output = null;
            string apiURL = "http://twitter.com/blocks/create/";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                        String.Format("{0}{1}.xml", apiURL, userId));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            if (userList != null)
                output = userList[0];

            return output;
        }

        /// <summary>
        /// Unblock the user matching the supplied userId
        /// </summary>
        /// <param name="userId">ID of the user to unblock</param>
        /// <returns>User object representing the user being unblocked</returns>
        public IUser UnblockUser(long userId)
        {
            IUser output = null;
            string apiURL = "http://twitter.com/blocks/destroy/";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                        String.Format("{0}{1}.xml", apiURL, userId));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            if (userList != null)
                output = userList[0];

            return output;
        }

        /// <summary>
        /// Checks to see if the authenticated user is blocking the user matching the supplied userId
        /// </summary>
        /// <param name="userId">ID of the user to check</param>
        /// <returns>TRUE if user is blocked, false if not</returns>
        public bool IsBlocked(long userId)
        {
            bool output = false;
            string apiURL = "http://twitter.com/blocks/exists/";
            string responseText = String.Empty;

            try
            {
                responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                                               String.Format("{0}{1}.xml", apiURL, userId));
            }
            catch(TwitterNETWebException tex)
            {
                //We catch this exception because if a block doesn't exist
                //API actually returns a 404 error, so we're catching to
                //parse and return proper bool value.
                if (tex.ResponseErrorText.Equals("You are not blocking this user."))
                    return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);
            
            if(userList != null && userList[0] != null)
                output = true;

            return output;
        }

        /// <summary>
        /// Gets a list of users being blocked by the authenticated user
        /// </summary>
        /// <returns>List of User objects representing the users being blocked</returns>
        public IList<IUser> GetBlockedUsers()
        {
            string apiURL = "http://twitter.com/blocks/blocking.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);

            return ResponseParser.ReturnUsers(responseText);
        }

        /// <summary>
        /// Gets a list of UserIDs of the users being blocked by the authenticated user
        /// </summary>
        /// <returns>List of UserIDs for the users being blocked</returns>
        public IList<long> GetBlockedUsersIds()
        {
            string apiURL = "http://twitter.com/blocks/blocking/ids.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);

            return ResponseParser.ReturnUserIDs(responseText);
        }
    }
}
