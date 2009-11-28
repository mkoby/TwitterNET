using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace TwitterNET
{
    //TODO: This file probably needs a better name
    internal class ResponseParser
    {
        public static IList<StatusMessage> ReturnStatuses(string responseText)
        {
            if (String.IsNullOrEmpty(responseText))
                return null;

            XElement _element = XElement.Parse(responseText);
            IList<StatusMessage> Output = new List<StatusMessage>();

            foreach (XElement statusElement in _element.DescendantsAndSelf("status"))
            {
                StatusMessage status = ParseStatusXML(statusElement);
                XElement userElement = statusElement.Element("user");

                if (userElement != null)
                    status.Author = ParseUserXml(userElement);

                Output.Add(status);
            }

            return Output;
        }

        public static IList<DirectMessage> ReturnDirectMsgs(string responseText)
        {
            if (String.IsNullOrEmpty(responseText))
                return null;

            XElement _element = XElement.Parse(responseText);
            IList<DirectMessage> Output = new List<DirectMessage>();

            foreach (XElement dm in _element.DescendantsAndSelf("direct_message"))
            {
                DirectMessage directMsg = ParseDirectMessageXML(dm);
                directMsg.Author = ParseUserXml(dm.Element("sender"));
                directMsg.Recipient = ParseUserXml(dm.Element("recipient"));
                Output.Add(directMsg);
            }

            return Output;
        }

        public static IList<IUser> ReturnUsers(string responseText)
        {
            if (String.IsNullOrEmpty(responseText))
                return null;

            XElement _element = XElement.Parse(responseText);
            IList<IUser> Output = new List<IUser>();

            foreach (XElement userElement in _element.DescendantsAndSelf("user"))
            {
                IUser user = ParseUserXml(userElement);
                XElement statusElement = userElement.Element("status");

                if (statusElement != null)
                    user.UserStatus = ParseStatusXML(statusElement);

                Output.Add(user);
            }

            return Output;
        }

        public static IList<long> ReturnUserIDs(string responseText)
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

        public static IList<SavedSearch> ReturnSavedSearches(string responseText)
        {
            if(String.IsNullOrEmpty(responseText))
                return null;

            IList<SavedSearch> Output = new List<SavedSearch>();
            XElement element = XElement.Parse(responseText);

            foreach (XElement ss in element.DescendantsAndSelf("saved_search"))
            {
                Output.Add(ParseSavedSearchXml(ss));
            }

            return Output;
        }

        // Internal Methods that handle the actual parsing of the XML
        // using XML to LINQ

        private static StatusMessage ParseStatusXML(XElement element)
        {
            StatusMessage Output = null;

            //TODO: Find a way to find and parse the <retweeted_status> element
            //  We want to parse it here because really the only difference
            //  between the a regular message & a retweet is that element
            var statusQuery = from statusElement in element.AncestorsAndSelf()
                        select new
                                   {
                                       id = statusElement.Element("id").Value,
                                       timestamp = statusElement.Element("created_at").Value,
                                       messageText = statusElement.Element("text").Value,
                                       source = statusElement.Element("source").Value,
                                       truncated = statusElement.Element("truncated").Value,
                                       inReplyStatusId = statusElement.Element("in_reply_to_status_id").Value,
                                       inReplyUserId = statusElement.Element("in_reply_to_user_id").Value,
                                   };

            var status = statusQuery.FirstOrDefault();

            return new StatusMessage(Convert.ToInt64(status.id), DateTime.ParseExact(status.timestamp, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture),
                                     status.messageText, null, status.inReplyStatusId.Equals(String.Empty) ? long.MinValue : Convert.ToInt64(status.inReplyStatusId),
                                     status.inReplyUserId.Equals(String.Empty) ? long.MinValue : Convert.ToInt64(status.inReplyUserId), status.source,
                                     Convert.ToBoolean(status.truncated));
        }

        private static IUser ParseUserXml(XElement element)
        {
            var userQuery = from userElement in element.AncestorsAndSelf()
                            select new
                                       {
                                           id = userElement.Element("id").Value,
                                           realName = userElement.Element("name").Value,
                                           screenName = userElement.Element("screen_name").Value,
                                           description = userElement.Element("description").Value,
                                           location = userElement.Element("location").Value,
                                           profileImageUrl = userElement.Element("profile_image_url").Value,
                                           website = userElement.Element("url").Value,
                                           protectedUpdates = userElement.Element("protected").Value,
                                           followerCount = userElement.Element("followers_count").Value,
                                           createdAt = userElement.Element("created_at").Value,
                                           favorites_count = userElement.Element("favourites_count").Value,
                                           utc_offset = userElement.Element("utc_offset").Value,
                                           time_zone = userElement.Element("time_zone").Value,
                                           profile_background_image_url = 
                                userElement.Element("profile_background_image_url").Value,
                                           profile_background_tile = userElement.Element("profile_background_tile").Value,
                                           statuses_count = userElement.Element("statuses_count").Value,
                                           notifications = userElement.Element("notifications").Value,
                                           following = userElement.Element("following").Value,
                                           profile_background_color = userElement.Element("profile_background_color").Value,
                                           profile_text_color = userElement.Element("profile_text_color").Value,
                                           profile_link_color = userElement.Element("profile_link_color").Value,
                                           profile_sidebar_fill_color = userElement.Element("profile_sidebar_fill_color").Value,
                                           profile_sidebar_border_color =
                                userElement.Element("profile_sidebar_border_color").Value
                                       };

            var user = userQuery.FirstOrDefault();

            long id = Convert.ToInt64(user.id);
            bool protectedUpdates = Convert.ToBoolean(user.protectedUpdates);
            long followerCount = Convert.ToInt64(user.followerCount);
            int favoritesCount = Convert.ToInt32(user.favorites_count);
            bool following = user.following.Equals(String.Empty) ? false : Convert.ToBoolean(user.following);
            bool notifications = user.notifications.Equals(String.Empty) ? false : Convert.ToBoolean(user.notifications);
            bool profileBackgroundTile = user.profile_background_tile.Equals(String.Empty) ? false : Convert.ToBoolean(user.profile_background_tile);
            long statuses_count = Convert.ToInt64(user.statuses_count);
            int utcOffset = user.utc_offset.Equals(String.Empty) ? 0 : Convert.ToInt32(user.utc_offset);

            return new User(id, user.realName, user.screenName, user.description, user.location, user.profileImageUrl,
                            user.website, protectedUpdates, followerCount,
                            DateTime.ParseExact(user.createdAt, "ddd MMM dd HH:mm:ss zzz yyyy",
                                                CultureInfo.InvariantCulture), favoritesCount, following, notifications,
                            user.profile_background_image_url, profileBackgroundTile, user.profile_background_color,
                            user.profile_link_color, user.profile_sidebar_fill_color, user.profile_sidebar_border_color,
                            user.profile_text_color, statuses_count, user.time_zone, utcOffset);
        }

        private static DirectMessage ParseDirectMessageXML(XElement element)
        {
            var dmQuery = from dm in element.AncestorsAndSelf("direct_message")
                          select
                              new
                                  {
                                      id = dm.Element("id").Value,
                                      timestamp = dm.Element("created_at").Value,
                                      messageText = dm.Element("text").Value
                                  };

            var directMsg = dmQuery.FirstOrDefault();

            return new DirectMessage(Convert.ToInt64(directMsg.id),
                                       DateTime.ParseExact(directMsg.timestamp, "ddd MMM dd HH:mm:ss zzz yyyy",
                                                           CultureInfo.InvariantCulture), directMsg.messageText, null, null);
        }

        private static SavedSearch ParseSavedSearchXml(XElement element)
        {
            var savedSearchQuery = from e in element.AncestorsAndSelf("saved_search")
                              select new
                                         {
                                             id = e.Element("id").Value,
                                             name = e.Element("name").Value,
                                             query = e.Element("query").Value,
                                             position = e.Element("position").Value,
                                             createdAt = e.Element("created_at").Value
                                         };

            var savedSearch = savedSearchQuery.FirstOrDefault();

            SavedSearch Output = new SavedSearch(Convert.ToInt64(savedSearch.id),
                                                 savedSearch.name, savedSearch.query,
                                                 savedSearch.position,
                                                 DateTime.ParseExact(savedSearch.createdAt,
                                                                     "ddd MMM dd HH:mm:ss zzz yyyy",
                                                                     CultureInfo.InvariantCulture));

            return Output;
        }
    }
}
