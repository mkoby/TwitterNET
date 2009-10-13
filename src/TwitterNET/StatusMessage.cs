using System;

namespace TwitterNET
{
	
	public class StatusMessage : Message
	{
		private long _replyToStatusID;
		private long _replyToUserID;
		private string _source;
		private bool _truncated;

		public StatusMessage(long ID, DateTime Timestamp, string MessageText, IUser Author, long ReplyToStatusID, long ReplyToUserID, string Source, bool Truncated)
			: base(ID, Timestamp, MessageText, Author)
		{
			_replyToStatusID = ReplyToStatusID;
			_replyToUserID = ReplyToUserID;
			_source = Source;
			_truncated = Truncated;
		}
		
		public long ReplyToStatusID
		{
			get { return _replyToStatusID; }
		}
		
		public long ReplyToUserID
		{
			get { return _replyToUserID; }
		}
		
		public string Source
		{
			get { return _source; }
		}
		
		public bool Truncated
		{
			get { return _truncated; }
		}
	}
}
