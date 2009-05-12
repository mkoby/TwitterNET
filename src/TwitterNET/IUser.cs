namespace TwitterNET
{
    public interface IUser
    {

        /*********************
         * Internal User Properties
         *********************/

        /// <summary>
        /// Does the user have extended properties
        /// </summary>
        bool HasExtendedProperties { get; }

        /// <summary>
        /// Extended user properties, when available in the returned data
        /// </summary>
        IExtendedUserProperties ExtendedUserProperties { get; set; }


        /*********************
         * User Properties
         *********************/ 

        /// <summary>
        /// System ID of User
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
        IStatus Status { get; set; }


        /*********************
         * User Methods
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