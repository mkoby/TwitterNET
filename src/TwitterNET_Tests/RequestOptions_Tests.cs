using System;
using NUnit.Framework;
using TwitterNET;

namespace TwitterNET_Tests
{
	
	[TestFixture]
	public class RequestOptions_Tests
	{
		private const string testUrl = "http://twitter.com/statuses/mentions.xml";
		private const string testUserUrl = "http://twitter.com/statuses/user_timeline.xml";
		private const long testStatusID = 144759968;
		private const string testUserName = "mkoby";
        private const long testUserID = 7263572;
		private const int testCount = 15;
		private const int testPage = 3;
        
		[Test]
		public void RequestOptions_Returns_PassedURL()
		{
			string testUrl = "http://twitter.com/statuses/mentions.xml";
			RequestOptions requestOptions = new RequestOptions();
			string builtUrl = requestOptions.BuildRequestUri(testUrl);
			
			Assert.AreEqual(testUrl, builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_URLForSinceID()
		{
			RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.SinceID, testStatusID);
			string builtUrl = requestOptions.BuildRequestUri(testUrl);
			
			Assert.AreEqual(String.Format("{0}?since_id={1}", testUrl, testStatusID), builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_URLForMaxID()
		{
			RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.MaxID, testStatusID);
			string builtUrl = requestOptions.BuildRequestUri(testUrl);
			
			Assert.AreEqual(String.Format("{0}?max_id={1}", testUrl, testStatusID), builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_URLForStatusID()
		{
			RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.ID, testStatusID);
			string builtUrl = requestOptions.BuildRequestUri(testUrl);
			
			Assert.AreEqual(String.Format("{0}?id={1}", testUrl, testStatusID), builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_URLForCount()
		{
			RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.Count, testCount);
			string builtUrl = requestOptions.BuildRequestUri(testUrl);
			
			Assert.AreEqual(String.Format("{0}?count={1}", testUrl, testCount), builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_URLForPageNumber()
		{
			RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.Page, testPage);
			string builtUrl = requestOptions.BuildRequestUri(testUrl);
			
			Assert.AreEqual(String.Format("{0}?page={1}", testUrl, testPage), builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_URLForUserID()
		{
			RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.UserID, testUserID);
			string builtUrl = requestOptions.BuildRequestUri(testUserUrl);
			
			Assert.AreEqual(String.Format("{0}?user_id={1}", testUserUrl, testUserID), builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_URLForUsername()
		{
			RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.Username, testUserName);
			string builtUrl = requestOptions.BuildRequestUri(testUserUrl);
			
			Assert.AreEqual(String.Format("{0}?screen_name={1}", testUserUrl, testUserName), builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_Exception_When_Not_UserTimeline_UserID()
		{
			RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.UserID, testUserID);
			string builtUrl = String.Empty;
			bool exceptionCaught = false;
			
			try
			{
				builtUrl = requestOptions.BuildRequestUri(testUrl);
			}
			catch(Exception twex)
			{
				exceptionCaught = twex.Message.Contains("only available for User Timeline requests");
			}
			
			Assert.IsTrue(exceptionCaught);
		}
		
		[Test]
		public void RequestOptions_Returns_Exception_When_Not_UserTimeline_Username()
		{
			RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.Username, testUserName);
			string builtUrl = String.Empty;
			bool exceptionCaught = false;
			
			try
			{
				builtUrl = requestOptions.BuildRequestUri(testUrl);
			}
			catch(Exception twex)
			{
				exceptionCaught = twex.Message.Contains("only available for User Timeline requests");
			}
			
			Assert.IsTrue(exceptionCaught);
		}
	}
}
