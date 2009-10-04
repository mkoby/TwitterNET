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
		private const string testScreenName = "mkoby";
        private const long testUserID = 7263572;
		private const int testCount = 15;
		private const int testPage = 3;
        
		[Test]
		public void RequestOptions_Returns_PassedURL()
		{
			string testUrl = "http://twitter.com/statuses/mentions.xml";
			StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			string builtUrl = statusRequestOptions.BuildRequestUri(testUrl);
			
			Assert.AreEqual(testUrl, builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_URLForSinceID()
		{
			StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.SinceID, testStatusID);
			string builtUrl = statusRequestOptions.BuildRequestUri(testUrl);
			
			Assert.AreEqual(String.Format("{0}?since_id={1}", testUrl, testStatusID), builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_URLForMaxID()
		{
			StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.MaxID, testStatusID);
			string builtUrl = statusRequestOptions.BuildRequestUri(testUrl);
			
			Assert.AreEqual(String.Format("{0}?max_id={1}", testUrl, testStatusID), builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_URLForStatusID()
		{
			StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.ID, testStatusID);
			string builtUrl = statusRequestOptions.BuildRequestUri(testUrl);
			
			Assert.AreEqual(String.Format("{0}?id={1}", testUrl, testStatusID), builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_URLForCount()
		{
			StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.Count, testCount);
			string builtUrl = statusRequestOptions.BuildRequestUri(testUrl);
			
			Assert.AreEqual(String.Format("{0}?count={1}", testUrl, testCount), builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_URLForPageNumber()
		{
			StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.Page, testPage);
			string builtUrl = statusRequestOptions.BuildRequestUri(testUrl);
			
			Assert.AreEqual(String.Format("{0}?page={1}", testUrl, testPage), builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_URLForUserID()
		{
			StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.UserID, testUserID);
			string builtUrl = statusRequestOptions.BuildRequestUri(testUserUrl);
			
			Assert.AreEqual(String.Format("{0}?user_id={1}", testUserUrl, testUserID), builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_URLForScreenName()
		{
			StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.ScreenName, testScreenName);
			string builtUrl = statusRequestOptions.BuildRequestUri(testUserUrl);
			
			Assert.AreEqual(String.Format("{0}?screen_name={1}", testUserUrl, testScreenName), builtUrl);
		}
		
		[Test]
		public void RequestOptions_Returns_Exception_When_Not_UserTimeline_UserID()
		{
			StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.UserID, testUserID);
			bool exceptionCaught = false;
			
			try
			{
				statusRequestOptions.BuildRequestUri(testUrl);
			}
			catch(Exception twex)
			{
                exceptionCaught = twex.Message.Contains("only available for certain kinds of requests");
			}
			
			Assert.IsTrue(exceptionCaught);
		}
		
		[Test]
		public void RequestOptions_Returns_Exception_When_Not_UserTimeline_Username()
		{
			StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.ScreenName, testScreenName);
			bool exceptionCaught = false;
			
			try
			{
				statusRequestOptions.BuildRequestUri(testUrl);
			}
			catch(Exception twex)
			{
                exceptionCaught = twex.Message.Contains("only available for certain kinds of requests");
			}
			
			Assert.IsTrue(exceptionCaught);
		}
	}
}
