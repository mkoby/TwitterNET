using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterNET
{
    public interface IStatus
    {
        /*********************
         * Status Properties
         *********************/ 

        /// <summary>
        /// Status ID
        /// </summary>
        long ID { get; }

        /// <summary>
        /// Date & Time status was posted
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// Date & Time status was posted in Twitter URL format
        /// </summary>
        string Twitter_Timestamp { get; }

        /// <summary>
        /// Actual text in status update
        /// </summary>
        string StatusText { get; }

        /// <summary>
        /// Where the status was sent from
        /// </summary>
        string Source { get; }

        /// <summary>
        /// Is the status over 140 characters?
        /// </summary>
        bool Truncated { get; }

        /// <summary>
        /// If the status is a reply, the ID of the status this status is the reply to
        /// </summary>
        string InReplyToStatusID { get; }
        
        /// <summary>
        /// If the status is a reply, the ID of the user who posted the message being replied to 
        /// </summary>
        string InReplyToUserID { get; }

        /// <summary>
        /// The user that posted this status
        /// </summary>
        IUser StatusUser { get; set; }


        /*********************
         * Status Methods
         *********************/ 

        /// <summary>
        /// Mark this status as a favorite
        /// </summary>
        void MarkFavorite();

        /// <summary>
        /// Reply to this particular status
        /// </summary>
        void ReplyToStatus();

        /// <summary>
        /// Delete the status if posted by the authenticated user
        /// </summary>
        void DeleteStatus();
    }
}
