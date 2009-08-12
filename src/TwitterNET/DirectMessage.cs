using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

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
