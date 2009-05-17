using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace TwitterNET
{
    internal class User : IUser
    {
        private long _id;
        private string _realName;
        private string _screenName;
        private string _description;
        private string _location;
        private string _profileImageUrl;
        private string _website;
        private bool _protected;
        private long _followerCount;

        private bool _hasExtendedProperties;
        private IExtendedUserProperties _extendedUserProperties;
        private IStatus _status;

        public User(long id, string realName, string screenName, string description, string location, string profileImageUrl, string website, bool protected_updates, long followerCount)
        {
            _id = id;
            _followerCount = followerCount;
            _protected = protected_updates;
            _website = website;
            _profileImageUrl = profileImageUrl;
            _location = location;
            _description = description;
            _screenName = screenName;
            _realName = realName;
        }

        public User(long id, string realName, string screenName, string description, string location, string profileImageUrl, string website, bool protected_updates, long followerCount, bool hasExtendedProperties, IExtendedUserProperties extendedUserProperties)
        {
            _id = id;
            _followerCount = followerCount;
            _protected = protected_updates;
            _website = website;
            _profileImageUrl = profileImageUrl;
            _location = location;
            _description = description;
            _screenName = screenName;
            _realName = realName;

            _hasExtendedProperties = hasExtendedProperties;
            _extendedUserProperties = extendedUserProperties;
        }


        /// <summary>
        /// System ID of StatusUser
        /// </summary>
        public long ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Real name of the user
        /// </summary>
        public string RealName
        {
            get { return _realName; }
        }

        /// <summary>
        /// Screen name of the user
        /// </summary>
        public string ScreenName
        {
            get { return _screenName; }
        }

        /// <summary>
        /// Description of the user
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Location of the user
        /// </summary>
        public string Location
        {
            get { return _location; }
        }

        /// <summary>
        /// Image URL of the user's profile image
        /// </summary>
        public string ProfileImageURL
        {
            get { return _profileImageUrl; }
        }

        /// <summary>
        /// Website of the user
        /// </summary>
        public string Website
        {
            get { return _website; }
        }

        /// <summary>
        /// Are the user's updates protected from public view
        /// </summary>
        public bool Protected
        {
            get { return _protected; }
        }

        /// <summary>
        /// The number of the followers the user has
        /// </summary>
        public long FollowerCount
        {
            get { return _followerCount; }
        }

        /// <summary>
        /// The user's most recent status update
        /// </summary>
        public IStatus UserStatus
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// Does the user have extended properties
        /// </summary>
        public bool HasExtendedProperties
        {
            get { return _hasExtendedProperties; }
        }

        /// <summary>
        /// Extended user properties, when available in the returned data
        /// </summary>
        public IExtendedUserProperties ExtendedUserProperties
        {
            get { return _extendedUserProperties; }
            set { _extendedUserProperties = value; }
        }

        /// <summary>
        /// Follow the user
        /// </summary>
        public void FollowUser()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unfollow the user
        /// </summary>
        public void UnfollowUser()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Turn on device updates for the user
        /// </summary>
        public void TurnOnDeviceUpdates()
        {
            throw new NotImplementedException();
        }
		
		internal static IEnumerable<IUser> Load(string xmlText)
		{
			var element = XElement.Parse(xmlText);
			IUser user = null;
			
			if(element.Name == "users")
			{
				foreach(var userElement in element.Descendants("user"))
				{
					user = null;
					user = ParseUserXml(userElement.ToString());
					
					foreach(var userStatusElement in userElement.Descendants("status"))
					{
						foreach(IStatus status in Status.Load(userStatusElement.ToString()))
							user.UserStatus = status;
					}
					
					yield return user;
				}
			}
			else if(element.Name == "user")
			{
				user = ParseUserXml(element.ToString());
				
				foreach(var userStatusElement in element.Descendants("status"))
				{
					foreach(IStatus status in Status.Load(userStatusElement.ToString()))
						user.UserStatus = status;
				}
				
				yield return user;
			}
		}        

        private static IUser ParseUserXml(string xmlText)
        {
            if (String.IsNullOrEmpty(xmlText))
                return null;  //Return NULL user to show it wsan't processed correctly

            IUser Output = null;
            IExtendedUserProperties extendedUserProperties = null;

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

                //Extended StatusUser Options
                DateTime created_at = DateTime.MinValue;
                int favorites_count = int.MinValue;
                int utc_offset = int.MinValue;
                string time_zone = String.Empty;
                string profile_background_image_url = String.Empty;
                bool profile_background_tile = false;
                long statuses_count = long.MinValue;
                bool notifications = false;
                bool following = false;
                string profile_background_color = String.Empty;
                string profile_text_color = String.Empty;
                string profile_link_color = String.Empty;
                string profile_sidebar_fill_color = String.Empty;
                string profile_sidebar_border_color = String.Empty;


                if (user.Element("profile_background_color") != null)
                {
                    //We have extended properties so we need to process these
                    created_at = DateTime.ParseExact((string)user.Element("created_at"), "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture); ;
                    favorites_count = (int)user.Element("favourites_count");
                    
                    try
                    {
                        utc_offset = (int)user.Element("utc_offset");
                    }
                    catch { utc_offset = int.MinValue; }

                    try
                    {
                        time_zone = (string)user.Element("time_zone");
                    }
                    catch { time_zone = String.Empty; }
                    
                    profile_background_image_url = (string)user.Element("profile_background_image_url");
                    profile_background_tile = (bool)user.Element("profile_background_tile");
                    statuses_count = (int)user.Element("statuses_count");

                    try
                    {
                        notifications = (bool)user.Element("notifications");
                    }
                    catch { notifications = false; }

                    try
                    {
                        following = (bool)user.Element("following");
                    }
                    catch { following = false; }

                    profile_background_color = (string)user.Element("profile_background_color");
                    profile_text_color = (string)user.Element("profile_text_color");
                    profile_link_color = (string)user.Element("profile_link_color");
                    profile_sidebar_fill_color = (string)user.Element("profile_sidebar_fill_color");
                    profile_sidebar_border_color = (string)user.Element("profile_sidebar_border_color");

                    extendedUserProperties = new ExtendedUserProperties(created_at, favorites_count, following, notifications, profile_background_image_url, profile_background_tile, 
                        profile_background_color, profile_link_color, profile_sidebar_fill_color, profile_sidebar_border_color, profile_text_color, statuses_count, time_zone, utc_offset);

                }

                if(extendedUserProperties == null)
                    Output = new User(id, realName, screenName, description, location, profileImageUrl, website, protected_updates, followerCount);
                else
                    Output = new User(id, realName, screenName, description, location, profileImageUrl, website, protected_updates, followerCount, true, extendedUserProperties);
            }

            return Output;
        }
    }
}