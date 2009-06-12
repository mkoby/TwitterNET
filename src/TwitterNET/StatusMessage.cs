using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

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
		
		internal static IEnumerable<StatusMessage> Load(string xmlText)
		{
			var element = XElement.Parse(xmlText);
			StatusMessage status = null;
			
			if(element.Name == "statuses")
			{
				foreach(var statusElement in element.Descendants("status"))
				{
					status = null;
					status = ParseStatusXML(statusElement.ToString());
					
					foreach(var statusUserElement in statusElement.Descendants("user"))
					{
						foreach(IUser user in User.Load(statusUserElement.ToString()))
							status.Author = user;
					}
					
					yield return status;
				}
			}
			else if(element.Name == "status")
			{
				status = ParseStatusXML(element.ToString());
				
				foreach(var statusUserElement in element.Descendants("user"))
				{
					foreach(IUser user in User.Load(statusUserElement.ToString()))
						status.Author = user;
				}
				
				yield return status;
			}
		}
		
		private static StatusMessage ParseStatusXML(string xmlText)
        {
            if (String.IsNullOrEmpty(xmlText))
                return null; //return NULL status to show it wasn't processed correctly

            StatusMessage Output = null;

            XElement statusXml = XElement.Parse(xmlText);

            var query = from c in statusXml.AncestorsAndSelf()
                    select c;

            foreach (var status in query)
            {
                long id = (long)status.Element("id");
                DateTime timestamp = DateTime.ParseExact((string)status.Element("created_at"), "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture);
                string messageText = (string)status.Element("text");
				
                string source = (string)status.Element("source");
                bool truncated = (bool)status.Element("truncated");
                string inReplyStatusID = (String.IsNullOrEmpty((string)status.Element("in_reply_to_status_id"))) ? long.MinValue.ToString() : (string)status.Element("in_reply_to_status_id");
                string inReplyUserID = (String.IsNullOrEmpty((string)status.Element("in_reply_to_user_id"))) ? long.MinValue.ToString() : (string)status.Element("in_reply_to_user_id");

                Output = new StatusMessage(id, timestamp, messageText, null, Convert.ToInt64(inReplyStatusID), Convert.ToInt64(inReplyUserID), source, truncated);
            }

            return Output;
        }
	}
}
