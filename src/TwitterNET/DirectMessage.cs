using System;

namespace TwitterNET
{
	public class DirectMessage : Message
	{
	    private IUser _recipient;
		
		public DirectMessage(long ID, DateTime Timestamp, string MessageText, IUser Author, IUser Recipient) 
            : base(ID, Timestamp, MessageText, Author)
		{
		    _recipient = Recipient;
		}

	    public IUser Recipient
	    {
            get { return _recipient; }
            internal set { _recipient = value; }
	    }
	}
}
