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

            return ResponseParser.ReturnSingleUser(responseText);
        }

        public IUser UnblockUser(long userId)
        {
            string apiURL = "http://twitter.com/blocks/destroy/";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler,
                        String.Format("{0}{1}.xml", apiURL, userId));

            return ResponseParser.ReturnSingleUser(responseText);
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
                    output = false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            IUser testparse = ResponseParser.ReturnSingleUser(responseText);

            if(testparse != null)
                output = true;

            return output;
        }

        public IList<IUser> GetBlockedUsers()
        {
            string apiURL = "http://twitter.com/blocks/blocking.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);

            return ResponseParser.ReturnListOfUsers(responseText);
        }

        public IList<long> GetBlockedUsersIds()
        {
            string apiURL = "http://twitter.com/blocks/blocking/ids.xml";
            string responseText = _requestHandler.MakeAPIRequest(_requestHandler, apiURL);

            return ResponseParser.ReturnListOfUserIDs(responseText);
        }
    }
}
