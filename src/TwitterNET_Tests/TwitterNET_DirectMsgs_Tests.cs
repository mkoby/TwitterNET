using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TwitterNET;

namespace TwitterNET_Tests
{
	
	[TestFixture]
	public class TwitterNET_DirectMsgs_Tests
	{
		
		private Twitter twitter = null;
        private const string TestUserName = "mkoby";
        private const long TestUserID = 7263572;
        private long minTestStatusID = long.MinValue,
					 maxTestStatusID = long.MinValue;

        [TestFixtureSetUp]
        public void TestFixture_Setup()
        {
        }

        [TestFixtureTearDown]
        public void TestFixture_TearDown()
        {}

        [SetUp]
        public void Test_Setup()
        {
            twitter = new Twitter("apitest4769", "testaccount");
        }

        [TearDown]
        public void Test_TearDown()
        {
            twitter = null;
        }

        [Test]
        public void GetSingleDirectMessage_Test()
        {
            DirectMessage dm = twitter.GetSingleDirectMessage(161115344);

            Assert.IsNotNull(dm);
        }
		
		[Test]
		public void GetLatestRecievedDirectMessages_Test()
		{
			IList<DirectMessage> directMsgs = null;
			directMsgs = twitter.GetDirectMessages(new RequestOptions());
			
			Assert.IsNotNull(directMsgs, "List of DMs is NULL, expected at least 1 DM");
			Assert.GreaterOrEqual(directMsgs.Count, 3);
		}

        [Test]
        public void GetSentDirectMessages_Test()
        {
            IList<DirectMessage> directMessages = null;
            directMessages = twitter.GetSentDirectMessages(new RequestOptions());

            Assert.IsNotNull(directMessages);
        }

        [Test]
        public void SendDirectMessage_Test()
        {
            string testMessage = String.Format("This is a test DM Sent at {0}",
                                               DateTime.Now.ToString("yyyy-MM-dd hh:mm"));

            DirectMessage dmSent = twitter.SendDirectMessage("apitest4769", testMessage);

            Assert.IsNotNull(dmSent, "Sent DM can not be NULL, this means DM was not successfully sent");
            Assert.AreEqual(testMessage, dmSent.MessageText);
        }

        [Test]
        public void DeleteDirectMessage_Test()
        {
            DirectMessage dmSent = twitter.SendDirectMessage("apitest4769", "This is a test DM to be deleted");
            Assert.IsNotNull(dmSent, "Test message must successfully send before testing DELETE");

            DirectMessage dmDeleted = twitter.DeleteDirectMessage(dmSent.ID);

            Assert.IsNotNull(dmDeleted, "Deleted DM can not be NULL, this means we did not successfully delete for some reason");
            Assert.AreEqual(dmSent.ID, dmDeleted.ID);
        }
	}
}
