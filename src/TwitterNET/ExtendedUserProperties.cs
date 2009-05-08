using System;

namespace TwitterNET
{
    class ExtendedUserProperties : IExtendedUserProperties
    {
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

        public ExtendedUserProperties(DateTime createAt, int favoritesCount, bool following, bool notifications, string profileBackgroundImageUrl, bool profileBackgroundTile, string profileBackgroundColor, string profileLinkColor, string profileSidebarFillColor, string profileSidebarBorderColor, string profileTextColor, long statusCount, string timeZone, int utcOffset)
        {
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

        public DateTime CreateAt
        {
            get { return _createAt; }
        }

        public int FavoritesCount
        {
            get { return _favoritesCount; }
        }

        public int UTCOffset
        {
            get { return _utcOffset; }
        }

        public string TimeZone
        {
            get { return _timeZone; }
        }

        public string ProfileBackgroundImageURL
        {
            get { return _profileBackgroundImageUrl; }
        }

        public bool ProfileBackgroundTile
        {
            get { return _profileBackgroundTile; }
        }

        public long StatusCount
        {
            get { return _statusCount; }
        }

        public bool Notifications
        {
            get { return _notifications; }
        }

        public bool Following
        {
            get { return _following; }
        }

        public string ProfileBackgroundColor
        {
            get { return _profileBackgroundColor; }
        }

        public string ProfileTextColor
        {
            get { return _profileTextColor; }
        }

        public string ProfileLinkColor
        {
            get { return _profileLinkColor; }
        }

        public string ProfileSidebarFillColor
        {
            get { return _profileSidebarFillColor; }
        }

        public string ProfileSidebarBorderColor
        {
            get { return _profileSidebarBorderColor; }
        }
    }
}