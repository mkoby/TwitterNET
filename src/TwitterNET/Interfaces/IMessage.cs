using System;

namespace TwitterNET
{
    public interface IMessage
    {
		/// <summary>
		/// ID of the message
		/// </summary>
		long ID { get; }
		
		/// <summary>
		/// Date/Time the message was sent
		/// </summary>
		DateTime Timestamp { get; }
		
		/// <summary>
		/// Actual text of the message
		/// </summary>
		string MessageText { get; }
		
		/// <summary>
		/// The author of the message
		/// </summary>
		IUser Author { get; }
	}
}