
using System;

namespace TwitterNET
{
	public class Message
	{
		private long _id;
		private DateTime _timestamp;
		private string _messageText;
		private IUser _author;
		
		public Message(long ID, DateTime Timestamp, string MessageText, IUser Author)
		{
			_id = ID;
			_timestamp = Timestamp;
			_messageText = MessageText;
			_author = Author;
		}
		
		public virtual long ID 
		{
			get { return _id; }
		}

		public virtual string MessageText
		{
			get { return _messageText; }
		}

		public virtual DateTime Timestamp
		{
			get { return _timestamp; }
		}	
		
		public virtual IUser Author 
		{
			get { return _author; }
			internal set { _author = value; }
		}
		
		
	}
}
