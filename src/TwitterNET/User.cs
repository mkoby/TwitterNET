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
		
		private DateTime _createAt;
        private int _favoritesCount;
        private bool _following;
        private bool _notifications;
        private string _profileBackgroundImageUrl;
        private bool _profileBackgroundTile;
        private string _profileBackgroundColor;
        private string _profileLinkColor;
        private string _profileSidebarFillColor;
        private string _profileSidebarBorderColor;
        private string _profileTextColor;
        private long _statusCount;
        private string _timeZone;
        private int _utcOffset;
        private StatusMessage _status;

        public User(long id, string realName, string screenName, string description, string location, 
		            string profileImageUrl, string website, bool protected_updates, long followerCount, 
		            DateTime createAt, int favoritesCount, bool following, bool notifications, 
		            string profileBackgroundImageUrl, bool profileBackgroundTile, string profileBackgroundColor, 
		            string profileLinkColor, string profileSidebarFillColor, string profileSidebarBorderColor, 
		            string profileTextColor, long statusCount, string timeZone, int utcOffset)
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
			
			_createAt = createAt;
            _utcOffset = utcOffset;
            _timeZone = timeZone;
            _statusCount = statusCount;
            _profileTextColor = profileTextColor;
            _profileSidebarBorderColor = profileSidebarBorderColor;
            _profileSidebarFillColor = profileSidebarFillColor;
            _profileLinkColor = profileLinkColor;
            _profileBackgroundColor = profileBackgroundColor;
            _profileBackgroundTile = profileBackgroundTile;
            _profileBackgroundImageUrl = profileBackgroundImageUrl;
            _following = following;
            _notifications = notifications;
            _favoritesCount = favoritesCount;
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
        public StatusMessage UserStatus
        {
            get { return _status; }
            set { _status = value; }
        }
		
		/// <summary>
		/// The date/time the user created their account
		/// </summary>
		public DateTime CreateAt 
		{ 
			get{ return _createAt; } 
		}
		
		/// <summary>
		/// Number of status' the user has favorited
		/// </summary>
        public int FavoritesCount 
		{ 
			get{ return _favoritesCount; } 
		} 
		
		/// <summary>
		/// The user's UTC Offset
		/// </summary>
        public int UTCOffset 
		{ 
			get{ return _utcOffset; } 
		}
		
		/// <summary>
		/// the user's time zone
		/// </summary>
        public string TimeZone 
		{ 
			get{ return _timeZone; } 
		}
		
		/// <summary>
		/// The user's background image URL
		/// </summary>
        public string ProfileBackgroundImageURL 
		{ 
			get{ return _profileBackgroundImageUrl; } 
		}
		
		/// <summary>
		/// Is the user's profile background tiled
		/// </summary>
        public bool ProfileBackgroundTile 
		{ 
			get{ return _profileBackgroundTile; } 
		}
		
		/// <summary>
		/// How many updates the user has posted
		/// </summary>
        public long StatusCount 
		{ 
			get{ return _statusCount; } 
		}
		
		/// <summary>
		/// Does the authenticated user get notifications on this user's updates?
		/// </summary>
        public bool Notifications 
		{ 
			get{ return _notifications; } 
		}
		
		/// <summary>
		/// Is the authenticated user following this user
		/// </summary>
        public bool Following 
		{ 
			get{ return _following; } 
		}
		
		/// <summary>
		/// User's profile background color
		/// </summary>
        public string ProfileBackgroundColor 
		{ 
			get{ return _profileBackgroundColor; } 
		}
		
		/// <summary>
		/// User's profile text color
		/// </summary>
        public string ProfileTextColor 
		{ 
			get{ return _profileTextColor; } 
		}
		
		/// <summary>
		/// User's profile link color
		/// </summary>
        public string ProfileLinkColor 
		{ 
			get{ return _profileLinkColor; } 
		}
		
		/// <summary>
		/// User's profile sidebar fill color
		/// </summary>
        public string ProfileSidebarFillColor 
		{ 
			get{ return _profileSidebarFillColor; } 
		}
		
		/// <summary>
		/// User's profile sidebar border color
		/// </summary>
        public string ProfileSidebarBorderColor 
		{ 
			get{ return _profileSidebarBorderColor; } 
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
						foreach(StatusMessage status in StatusMessage.Load(userStatusElement.ToString()))
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
					foreach(StatusMessage status in StatusMessage.Load(userStatusElement.ToString()))
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
				
                string profile_background_color = (string)user.Element("profile_background_color");;
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
		
		public override string ToString ()
		{
			return string.Format("[User: ID={0}, RealName={1}, ScreenName={2}, Description={3}, Location={4}, ProfileImageURL={5}, Website={6}, Protected={7}, FollowerCount={8}, UserStatus={9}, CreateAt={10}, FavoritesCount={11}, UTCOffset={12}, TimeZone={13}, ProfileBackgroundImageURL={14}, ProfileBackgroundTile={15}, StatusCount={16}, Notifications={17}, Following={18}, ProfileBackgroundColor={19}, ProfileTextColor={20}, ProfileLinkColor={21}, ProfileSidebarFillColor={22}, ProfileSidebarBorderColor={23}]", ID, RealName, ScreenName, Description, Location, ProfileImageURL, Website, Protected, FollowerCount, UserStatus, CreateAt, FavoritesCount, UTCOffset, TimeZone, ProfileBackgroundImageURL, ProfileBackgroundTile, StatusCount, Notifications, Following, ProfileBackgroundColor, ProfileTextColor, ProfileLinkColor, ProfileSidebarFillColor, ProfileSidebarBorderColor);
		}

    }
}