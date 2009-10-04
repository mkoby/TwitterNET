using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TwitterNET
{
    //TODO: This file probably needs a better name
    internal class ResponseParser
    {
        public static StatusMessage ReturnSingleStatus(string responseText)
        {
            if(String.IsNullOrEmpty(responseText))
                return null;

            XElement _element = XElement.Parse(responseText);
            StatusMessage Output = ParseStatusXML(_element.ToString());

            //Check for the user element, parse if exists
            XElement userElement = _element.Element("user");
            
            if(userElement != null)
                Output.Author = ParseUserXml(_element.Element("user").ToString());
            
            _element = null;

            return Output;
        }

        public static IList<StatusMessage> ReturnListOfStatuses(string responseText)
        {
            if (String.IsNullOrEmpty(responseText))
                return null;

            XElement _element = XElement.Parse(responseText);
            IList<StatusMessage> Output = new List<StatusMessage>();

            foreach (XElement status in _element.Descendants("status"))
            {
                Output.Add(ReturnSingleStatus(status.ToString()));
            }

            _element = null;

            return Output;
        }

        public static DirectMessage ReternSingleDirectMsg(string responseText)
        {
            if (String.IsNullOrEmpty(responseText))
                return null;

            XElement _element = XElement.Parse(responseText);
            DirectMessage Output = ParseDirectMessageXML(_element.ToString());
            Output.Author = ReturnSingleUser(_element.Element("sender").ToString());
            Output.Recipient = ReturnSingleUser(_element.Element("recipient").ToString());
            _element = null;

            return Output;
        }

        public static IList<DirectMessage> ReturnListofDirectMsgs(string responseText)
        {
            if (String.IsNullOrEmpty(responseText))
                return null;

            XElement _element = XElement.Parse(responseText);
            IList<DirectMessage> Output = new List<DirectMessage>();

            foreach (XElement dm in _element.Descendants("direct_message"))
            {
                Output.Add(ReternSingleDirectMsg(dm.ToString()));
            }

            _element = null;

            return Output;
        }

        public static IUser ReturnSingleUser(string responseText)
        {
            if (String.IsNullOrEmpty(responseText))
                return null;

            XElement _element = XElement.Parse(responseText);
            IUser Output = ParseUserXml(_element.ToString());
            
            //Check for a Status element and get the element if nescessary
            XElement statusElement = _element.Element("status");

            if (statusElement != null)
                Output.UserStatus = ReturnSingleStatus(statusElement.ToString());

            _element = null;

            return Output;
        }

        public static IList<IUser> ReturnListOfUsers(string responseText)
        {
            if (String.IsNullOrEmpty(responseText))
                return null;

            XElement _element = XElement.Parse(responseText);
            IList<IUser> Output = new List<IUser>();

            foreach (XElement status in _element.Descendants("user"))
            {
                Output.Add(ReturnSingleUser(status.ToString()));
            }

            _element = null;

            return Output;
        }

        public static IList<long> ReturnListOfUserIDs(string responseText)
        {
            if (String.IsNullOrEmpty(responseText))
                return null;

            IList<long> Output = new List<long>();
            XElement responseElement = XElement.Parse(responseText);

            var idQuery = from i in responseElement.Descendants("id")
                          select i;

            foreach (var query in idQuery)
            {
                Output.Add(long.Parse(query.Value));
            }  

            return Output;
        }

        public static IDictionary<string, object> ReturnHashDictionary(string responseText)
        {
            if(String.IsNullOrEmpty(responseText))
                return null;

            IDictionary<string, object> Output = null;
            XElement hashXml = XElement.Parse(responseText);

            var query = from c in hashXml.Descendants()
                        select c;
            
            if(query.Count() > 0)
                Output = new Dictionary<string, object>();

            foreach(var hash in query)
            {
                Output.Add(hash.Name.ToString(), hash.Value);
            }

            return Output;
        }

        // Internal Methods that handle the actual parsing of the XML
        // using XML to LINQ

        private static StatusMessage ParseStatusXML(string xmlText)
        {
            if (String.IsNullOrEmpty(xmlText))
                return null; //return NULL status to show it wasn't processed correctly

            StatusMessage Output = null;

            XElement statusXml = XElement.Parse(xmlText);

            var query = from c in statusXml.AncestorsAndSelf()
                    select c;

            foreach (var status in query)
            {
                long id = (long)status.Element("id");
                DateTime timestamp = DateTime.ParseExact((string)status.Element("created_at"), "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture);
                string messageText = (string)status.Element("text");
				
                string source = (string)status.Element("source");
                bool truncated = (bool)status.Element("truncated");
                string inReplyStatusID = (String.IsNullOrEmpty((string)status.Element("in_reply_to_status_id"))) ? long.MinValue.ToString() : (string)status.Element("in_reply_to_status_id");
                string inReplyUserID = (String.IsNullOrEmpty((string)status.Element("in_reply_to_user_id"))) ? long.MinValue.ToString() : (string)status.Element("in_reply_to_user_id");

                Output = new StatusMessage(id, timestamp, messageText, null, Convert.ToInt64(inReplyStatusID), Convert.ToInt64(inReplyUserID), source, truncated);
            }

            return Output;
        }

        private static IUser ParseUserXml(string xmlText)
        {
            if (String.IsNullOrEmpty(xmlText))
                return null;  //Return NULL user to show it wsan't processed correctly

            IUser Output = null;
            XElement statusXml = XElement.Parse(xmlText);

            var userQuery = from u in statusXml.AncestorsAndSelf()
                            select u;

            foreach (var user in userQuery)
            {
                long id = (long)user.Element("id");
                string realName = (string)user.Element("name");
                string screenName = (string)user.Element("screen_name");
                string description = (string)user.Element("description");
                string location = (string)user.Element("location");
                string profileImageUrl = (string)user.Element("profile_image_url");
                string website = (string)user.Element("url");
                bool protected_updates = (bool)user.Element("protected");
                long followerCount = (long)user.Element("followers_count");
                DateTime created_at = DateTime.ParseExact((string)user.Element("created_at"), "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture);
                int favorites_count = (int)user.Element("favourites_count");
                int utc_offset;

                try
                {
                    utc_offset = (int)user.Element("utc_offset");
                }
                catch { utc_offset = int.MinValue; }

                string time_zone;

                try
                {
                    time_zone = (string)user.Element("time_zone");
                }
                catch { time_zone = String.Empty; }

                string profile_background_image_url = (string)user.Element("profile_background_image_url");
                bool profile_background_tile = (bool)user.Element("profile_background_tile");
                long statuses_count = (int)user.Element("statuses_count");
                bool notifications;

                try
                {
                    notifications = (bool)user.Element("notifications");
                }
                catch { notifications = false; }

                bool following;

                try
                {
                    following = (bool)user.Element("following");
                }
                catch { following = false; }

                string profile_background_color = (string)user.Element("profile_background_color"); ;
                string profile_text_color = (string)user.Element("profile_text_color");
                string profile_link_color = (string)user.Element("profile_link_color");
                string profile_sidebar_fill_color = (string)user.Element("profile_sidebar_fill_color");
                string profile_sidebar_border_color = (string)user.Element("profile_sidebar_border_color");

                Output = new User(id, realName, screenName, description, location, profileImageUrl, website,
                                  protected_updates, followerCount, created_at, favorites_count, following,
                                  notifications, profile_background_image_url, profile_background_tile,
                                  profile_background_color, profile_link_color, profile_sidebar_fill_color,
                                  profile_sidebar_border_color, profile_text_color, statuses_count, time_zone,
                                  utc_offset);
            }

            return Output;
        }

        private static DirectMessage ParseDirectMessageXML(string xmlText)
        {
            if (String.IsNullOrEmpty(xmlText))
                return null; //return NULL directMsg to show it wasn't processed correctly

            DirectMessage Output = null;
            XElement directMsgXml = XElement.Parse(xmlText);
            var query = from c in directMsgXml.AncestorsAndSelf()
                        select c;

            foreach (var directMsg in query)
            {
                long id = (long)directMsg.Element("id");
                DateTime timestamp = DateTime.ParseExact((string)directMsg.Element("created_at"), "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture);
                string messageText = (string)directMsg.Element("text");

                Output = new DirectMessage(id, timestamp, messageText, null, null);
            }

            return Output;
        }
    }
}
