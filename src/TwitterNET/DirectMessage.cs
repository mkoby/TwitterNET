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
		
		private static IUser GetUser(XElement statusElement, string userType)
		{
			IUser Output = null;
			
			foreach(var statusUserElement in statusElement.Descendants(userType))
			{

				foreach(IUser user in User.Load(statusUserElement.ToString()))
					Output = user;
			}
			
			return Output;
		}
		
		private static DirectMessage ParseStatusXML(string xmlText)
        {
            if (String.IsNullOrEmpty(xmlText))
                return null; //return NULL directMsg to show it wasn't processed correctly

            DirectMessage Output = null;
            XElement directMsgXml = XElement.Parse(xmlText);
            var query = from c in directMsgXml.AncestorsAndSelf()
                    select c;

            foreach (var directMsg in query)
            {
                long id = (long)directMsg.Element("id");
                DateTime timestamp = DateTime.ParseExact((string)directMsg.Element("created_at"), "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture);
                string messageText = (string)directMsg.Element("text");
				
				Output = new DirectMessage(id, timestamp, messageText, null, null);
            }

            return Output;
        }
		
		private static DirectMessage GetDirectMessage(XElement element)
		{
			DirectMessage Output = null;
			
			Output = ParseStatusXML(element.ToString());					
			Output.Author = GetUser(element, "sender");
			Output.Recipient = GetUser(element, "recipient");
			
			return Output;
		}
		
		internal static IEnumerable<DirectMessage> Load(string xmlText)
		{
			var element = XElement.Parse(xmlText);
			DirectMessage directMsg = null;
			
			if(element.Name == "direct-messages")
			{
				foreach(var directMsgElement in element.Descendants("direct_message"))
				{
					directMsg = GetDirectMessage(directMsgElement);
					
					yield return directMsg;
				}
			}
			else if(element.Name == "direct_message")
			{
				directMsg = GetDirectMessage(element);
				
				yield return directMsg;
			}
		}
	}
}
