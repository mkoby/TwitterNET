using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace TwitterNET
{
    internal class Status : IStatus
    {
        private long _id;
        private DateTime _timestamp;
        private string _statusText;
        private string _source;
        private bool _truncated;
        private string _inReplyToStatusId;
        private string _inReplyToUserId;
        private IUser _statusUser;

        public Status(long id, DateTime timestamp, string statusText, string source, bool truncated, string inReplyToStatusId, string inReplyToUserId, IUser user)
        {
            _id = id;
            _statusUser = user;
            _inReplyToUserId = inReplyToUserId;
            _inReplyToStatusId = inReplyToStatusId;
            _truncated = truncated;
            _source = source;
            _statusText = statusText;
            _timestamp = timestamp;
        }

        /// <summary>
        /// Status ID
        /// </summary>
        public long ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Date & Time status was posted
        /// </summary>
        public DateTime Timestamp
        {
            get { return _timestamp; }
        }

        /// <summary>
        /// Date & Time status was posted in Twitter URL format
        /// </summary>
        public string Twitter_Timestamp
        {
            get { return _timestamp.ToString("ddd MMM dd HH:mm:ss zzz yyyy"); }
        }

        /// <summary>
        /// Actual text in status update
        /// </summary>
        public string StatusText
        {
            get { return _statusText; }
        }

        /// <summary>
        /// Where the status was sent from
        /// </summary>
        public string Source
        {
            get { return _source; }
        }

        /// <summary>
        /// Is the status over 140 characters?
        /// </summary>
        public bool Truncated
        {
            get { return _truncated; }
        }

        /// <summary>
        /// If the status is a reply, the ID of the status this status is the reply to
        /// </summary>
        public string InReplyToStatusID
        {
            get { return _inReplyToStatusId; }
        }

        /// <summary>
        /// If the status is a reply, the ID of the user who posted the message being replied to 
        /// </summary>
        public string InReplyToUserID
        {
            get { return _inReplyToUserId; }
        }

        /// <summary>
        /// The user that posted this status
        /// </summary>
        public IUser StatusUser
        {
            get { return _statusUser; }
            set { _statusUser = value; }
        }

        /// <summary>
        /// Mark this status as a favorite
        /// </summary>
        public void MarkFavorite()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reply to this particular status
        /// </summary>
        public void ReplyToStatus()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete the status if posted by the authenticated user
        /// </summary>
        public void DeleteStatus()
        {
            throw new NotImplementedException();
        }
		
		internal static IEnumerable<IStatus> Load(string xmlText)
		{
			var element = XElement.Parse(xmlText);
			IStatus status = null;
			
			if(element.Name == "statuses")
			{
				foreach(var statusElement in element.Descendants("status"))
				{
					status = null;
					status = ParseStatusXML(statusElement.ToString());
					
					foreach(var statusUserElement in statusElement.Descendants("user"))
					{
						foreach(IUser user in User.Load(statusUserElement.ToString()))
							status.StatusUser = user;
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
						status.StatusUser = user;
				}
				
				yield return status;
			}
		}
		
		private static IStatus ParseStatusXML(string xmlText)
        {
            if (String.IsNullOrEmpty(xmlText))
                return null; //return NULL status to show it wasn't processed correctly

            IStatus Output = null;

            XElement statusXml = XElement.Parse(xmlText);

            var query = from c in statusXml.AncestorsAndSelf()
                    select c;

            foreach (var status in query)
            {
                long Id = (long)status.Element("id");
                DateTime Timestamp = DateTime.ParseExact((string)status.Element("created_at"), "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture);
                string statusText = (string)status.Element("text");
                string source = (string)status.Element("source");
                bool truncated = (bool)status.Element("truncated");
                string inReplyStatusID = (string)status.Element("in_reply_to_status_id");
                string inReplyUserID = (string)status.Element("in_reply_to_user_id");

                Output = new Status(Id, Timestamp, statusText, source, truncated, inReplyStatusID, inReplyUserID, null);
            }

            return Output;
        }
    }
}