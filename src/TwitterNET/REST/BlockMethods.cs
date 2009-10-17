using System;
using System.Collections.Generic;

namespace TwitterNET
{
    public partial class Twitter
    {
        public IUser BlockUser(long userId)
        {
            string apiURL = "http://twitter.com/blocks/create/";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                        String.Format("{0}{1}.xml", apiURL, userId));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            return userList[0];
        }

        public IUser UnblockUser(long userId)
        {
            string apiURL = "http://twitter.com/blocks/destroy/";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                        String.Format("{0}{1}.xml", apiURL, userId));
            IList<IUser> userList = ResponseParser.ReturnUsers(responseText);

            return userList[0];
        }

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

        public IList<IUser> GetBlockedUsers()
        {
            string apiURL = "http://twitter.com/blocks/blocking.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);

            return ResponseParser.ReturnUsers(responseText);
        }

        public IList<long> GetBlockedUsersIds()
        {
            string apiURL = "http://twitter.com/blocks/blocking/ids.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);

            return ResponseParser.ReturnUserIDs(responseText);
        }
    }
}
