using System;

namespace TwitterNET
{
    public interface IUser
    {
        /*********************
         * StatusUser Properties
         *********************/ 

        /// <summary>
        /// System ID of StatusUser
        /// </summary>
        long ID { get; }

        /// <summary>
        /// Real name of the user
        /// </summary>
        string RealName { get; }

        /// <summary>
        /// Screen name of the user
        /// </summary>
        string ScreenName { get; }

        /// <summary>
        /// Description of the user
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Location of the user
        /// </summary>
        string Location { get; }

        /// <summary>
        /// Image URL of the user's profile image
        /// </summary>
        string ProfileImageURL { get; }

        /// <summary>
        /// Website of the user
        /// </summary>
        string Website { get; }

        /// <summary>
        /// Are the user's updates protected from public view
        /// </summary>
        bool Protected { get; }

        /// <summary>
        /// The number of the followers the user has
        /// </summary>
        long FollowerCount { get; }

        /// <summary>
        /// The user's most recent status update
        /// </summary>
        IStatus UserStatus { get; set; }
		
		/// <summary>
		/// The date/time the user created their account
		/// </summary>
		DateTime CreateAt { get; }
		
		/// <summary>
		/// Number of status' the user has favorited
		/// </summary>
        int FavoritesCount { get; } 
		
		/// <summary>
		/// The user's UTC Offset
		/// </summary>
        int UTCOffset { get; }
		
		/// <summary>
		/// the user's time zone
		/// </summary>
        string TimeZone { get; }
		
		/// <summary>
		/// The user's background image URL
		/// </summary>
        string ProfileBackgroundImageURL { get; }
		
		/// <summary>
		/// Is the user's profile background tiled
		/// </summary>
        bool ProfileBackgroundTile { get; }
		
		/// <summary>
		/// How many updates the user has posted
		/// </summary>
        long StatusCount { get; }
		
		/// <summary>
		/// Does the authenticated user get notifications on this user's updates?
		/// </summary>
        bool Notifications { get; }
		
		/// <summary>
		/// Is the authenticated user following this user
		/// </summary>
        bool Following { get; }
		
		/// <summary>
		/// User's profile background color
		/// </summary>
        string ProfileBackgroundColor { get; }
		
		/// <summary>
		/// User's profile text color
		/// </summary>
        string ProfileTextColor { get; }
		
		/// <summary>
		/// User's profile link color
		/// </summary>
        string ProfileLinkColor { get; } 
		
		/// <summary>
		/// User's profile sidebar fill color
		/// </summary>
        string ProfileSidebarFillColor { get; }
		
		/// <summary>
		/// User's profile sidebar border color
		/// </summary>
        string ProfileSidebarBorderColor { get; }


        /*********************
         * StatusUser Methods
         *********************/ 

        /// <summary>
        /// Follow the user
        /// </summary>
        void FollowUser();

        /// <summary>
        /// Unfollow the user
        /// </summary>
        void UnfollowUser();

        /// <summary>
        /// Turn on device updates for the user
        /// </summary>
        void TurnOnDeviceUpdates();
    }
}