﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TwitterNET;

namespace TwitterNET_Tests
{
    [TestFixture]
    public class TwitterNET_UserTimeLine_Tests
    {
        private Twitter twitter = null;
        private const string TestUserName = "mkoby";
        private const long TestUserID = 7263572;
        private long minTestStatusID = long.MinValue,
					 maxTestStatusID = long.MinValue;

        [TestFixtureSetUp]
        public void TestFixture_Setup()
        {
            try
            {
                twitter = new Twitter("apitest4769", "testaccount");

                //Ensure we're following the user we'll need in some tests
                if (!twitter.CheckFriendship("apitest4769", "mkoby"))
                    twitter.FollowUser("mkoby", false);

                StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
                statusRequestOptions.Add(StatusRequestOptionNames.Page, 3);
                statusRequestOptions.Add(StatusRequestOptionNames.ScreenName, TestUserName);
                IList<StatusMessage> friendsTimeline = twitter.GetUserTimeline(statusRequestOptions);

                if (friendsTimeline != null && friendsTimeline.Count > 0)
                {
                    minTestStatusID = friendsTimeline.Min(status => status.ID);
					maxTestStatusID = friendsTimeline.Max(status => status.ID);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
			
			twitter = null;
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
        public void Get_UserTimeline_userName()
        {
			StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.ScreenName, TestUserName);
			
            IList<StatusMessage> statusList = twitter.GetUserTimeline(statusRequestOptions);

            foreach (var status in statusList)
            {
                Console.WriteLine("{0}: {1}", status.Author.ScreenName, status.MessageText);
            }
			
			var userCount = from c in statusList
							where c.Author.ScreenName == TestUserName
							select c;
			
            Assert.AreEqual(20, userCount.Count());
        }

        [Test]
        public void Get_UserTimeline_longID()
        {
            StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.UserID, TestUserID);
			
            IList<StatusMessage> statusList = twitter.GetUserTimeline(statusRequestOptions);

            foreach (var status in statusList)
            {
                Console.WriteLine("{0}: {1}", status.Author.ScreenName, status.MessageText);
            }
			
			var idCount = from c in statusList
							where c.Author.ID == TestUserID
							select c;

            Assert.AreEqual(20, idCount.Count());
        }
        
        [Test]
        public void Get_UserTimeline_SinceStatusID()
        {
			//We're going to run this for a specific user 
			//because the test account doesn't post updates
			//very often and we can't control test-run order
            StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.ScreenName, TestUserName);
			statusRequestOptions.Add(StatusRequestOptionNames.SinceID, minTestStatusID);
			
            IList<StatusMessage> statusList = twitter.GetUserTimeline(statusRequestOptions);

            //Make sure we got at least 1 status back
            Assert.IsNotEmpty((ICollection)statusList, "Status list was empty, expected at least 1 status returned");

            long returnedStatusID = (long)statusList.Min(status => status.ID);

            Console.WriteLine("TestStatusID: {0}\nMinStatusID: {1}", minTestStatusID, returnedStatusID);
            Assert.GreaterOrEqual(returnedStatusID, minTestStatusID);
        }

        [Test]
        public void Get_UserTimeline_MaxStatusID()
        {			
			//We're going to run this for a specific user 
			//because the test account doesn't post updates
			//very often and we can't control test-run order
            StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.ScreenName, TestUserName);
			statusRequestOptions.Add(StatusRequestOptionNames.MaxID, maxTestStatusID);
			
            IList<StatusMessage> statusList = twitter.GetUserTimeline(statusRequestOptions);

            //Make sure we got at least 1 status back
            Assert.IsNotEmpty((ICollection)statusList, "Status list was empty, expected at least 1 status returned");

            long returnedStatusID = (long)statusList.Max(status => status.ID);

            Console.WriteLine("TestStatusID: {0}\nMaxStatusID: {1}", maxTestStatusID, returnedStatusID);
            Assert.LessOrEqual(returnedStatusID, maxTestStatusID);
        }

        [Test]
        public void Get_UsersFriends_Test()
        {
            IList<IUser> friendsStatusList = twitter.GetUsersFriends(new StatusRequestOptions());

            Assert.IsNotEmpty((ICollection)friendsStatusList);

            var bizUser = from user in friendsStatusList
                          where user.ScreenName.Equals("biz", StringComparison.OrdinalIgnoreCase)
						  select user;
			
			Assert.AreEqual(1, bizUser.Count());
        }
		
		[Test]
		public void Get_UsersFollowers_Test()
		{
			IList<IUser> followersStatusList = twitter.GetUsersFollowers(new StatusRequestOptions());
			
			Assert.IsNotEmpty((ICollection)followersStatusList);
			
			var mkobyUser = from user in followersStatusList
							where user.ScreenName.Equals("mkoby", StringComparison.OrdinalIgnoreCase)
							select user;
			
			Assert.AreEqual(1, mkobyUser.Count());
		}
    }
}

