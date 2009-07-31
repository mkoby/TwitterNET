using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterNET
{
    //TODO: This file probably needs a better name
    internal class Parser
    {
        public static StatusMessage ReturnSingleStatus(string responseText)
        {
            StatusMessage Output = null;

            if (!string.IsNullOrEmpty(responseText))
            {
                foreach (StatusMessage status in StatusMessage.Load(responseText))
                {
                    Output = status;
                    break; //we only want the first status (there should only be 1 anyway)
                }
            }

            return Output;
        }

        public static IList<StatusMessage> ReturnListOfStatuses(string responseText)
        {
            IList<StatusMessage> Output = new List<StatusMessage>();

            if (!string.IsNullOrEmpty(responseText))
            {
                foreach (StatusMessage status in StatusMessage.Load(responseText))
                    Output.Add(status);
            }

            return Output;
        }

        public static DirectMessage ReternSingleDirectMsg(string responseText)
        {
            DirectMessage Output = null;

            if (!String.IsNullOrEmpty(responseText))
            {
                foreach (DirectMessage dm in DirectMessage.Load(responseText))
                {
                    Output = dm;
                    break; //we only want the first DM (there should only be 1 anyway)
                }
            }

            return Output;
        }

        public static IList<DirectMessage> ReturnListofDirectMsgs(string responseText)
        {
            IList<DirectMessage> Output = new List<DirectMessage>();

            if (!string.IsNullOrEmpty(responseText))
            {
                foreach (DirectMessage message in DirectMessage.Load(responseText))
                    Output.Add(message);
            }

            return Output;
        }

        public static IUser ReturnSingleUser(string responseText)
        {
            IUser Output = null;

            if (!string.IsNullOrEmpty(responseText))
            {
                foreach (IUser user in User.Load(responseText))
                {
                    Output = user;
                    break; //we only want the first user (there should only be 1 anyway)
                }
            }

            return Output;
        }

        public static IList<IUser> ReturnListOfUsers(string responseText)
        {
            IList<IUser> Output = new List<IUser>();

            if (!string.IsNullOrEmpty(responseText))
            {
                foreach (IUser user in User.Load(responseText))
                    Output.Add(user);
            }

            return Output;
        }
    }
}
