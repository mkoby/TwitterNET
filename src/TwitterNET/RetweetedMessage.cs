using System;

namespace TwitterNET
{
    public class RetweetedMessage : StatusMessage
    {
        private StatusMessage _retweetedStatus;

        public RetweetedMessage(long ID, DateTime Timestamp, string MessageText, IUser Author, long ReplyToStatusID, long ReplyToUserID, string Source, bool Truncated, StatusMessage RetweetedStatus) 
            : base(ID, Timestamp, MessageText, Author, ReplyToStatusID, ReplyToUserID, Source, Truncated)
        {
            _retweetedStatus = RetweetedStatus;
        }

        public StatusMessage RetweetedStatus
        {
            get { return _retweetedStatus; }
        }
    }
}