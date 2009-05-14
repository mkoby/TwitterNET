using System;
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
        private long TestStatusID = long.MinValue;

        [TestFixtureSetUp]
        public void TestFixture_Setup()
        {
            try
            {
                twitter = new Twitter("apitest4769", "testaccount");
                IList<IStatus> friendsTimeline = twitter.GetFriendsTimeline(new RequestOptions());

                if (friendsTimeline != null && friendsTimeline.Count > 0)
                {
                    TestStatusID = (long) friendsTimeline.Min(status => status.ID);
					Random rnd = new Random(DateTime.Now.Millisecond);
					TestStatusID = TestStatusID - rnd.Next(10000, 250000);
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
			RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.ScreenName, TestUserName);
			
            IList<IStatus> statusList = twitter.GetUserTimeline(requestOptions);

            foreach (var status in statusList)
            {
                Console.WriteLine("{0}: {1}", status.StatusUser.ScreenName, status.StatusText);
            }
			
			var userCount = from c in statusList
							where c.StatusUser.ScreenName == TestUserName
							select c;
			
            Assert.AreEqual(20, userCount.Count());
        }

        [Test]
        public void Get_UserTimeline_longID()
        {
            RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.UserID, TestUserID);
			
            IList<IStatus> statusList = twitter.GetUserTimeline(requestOptions);

            foreach (var status in statusList)
            {
                Console.WriteLine("{0}: {1}", status.StatusUser.ScreenName, status.StatusText);
            }
			
			var idCount = from c in statusList
							where c.StatusUser.ID == TestUserID
							select c;

            Assert.AreEqual(20, idCount.Count());
        }
        
        [Test]
        public void Get_UserTimeline_SinceStatusID()
        {
            RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.SinceID, TestStatusID);
			
            IList<IStatus> statusList = twitter.GetUserTimeline(requestOptions);

            //Make sure we got at least 1 status back
            Assert.IsNotEmpty((ICollection)statusList, "Status list was empty, expected at least 1 status returned");

            long returnedStatusID = (long)statusList.Min(status => status.ID);

            Console.WriteLine("TestStatusID: {0}\nMinStatusID: {1}", TestStatusID, returnedStatusID);
            Assert.Greater(returnedStatusID, TestStatusID);
        }

        [Test]
        public void Get_UserTimeline_MaxStatusID()
        {
            RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.MaxID, TestStatusID);
			
            IList<IStatus> statusList = twitter.GetUserTimeline(requestOptions);

            //Make sure we got at least 1 status back
            Assert.IsNotEmpty((ICollection)statusList, "Status list was empty, expected at least 1 status returned");

            long returnedStatusID = (long)statusList.Max(status => status.ID);

            Console.WriteLine("TestStatusID: {0}\nMaxStatusID: {1}", TestStatusID, returnedStatusID);
            Assert.Less(returnedStatusID, TestStatusID);
        }

        [Test]
        public void Get_UsersFriends_Test()
        {
            IList<IUser> friendsStatusList = twitter.GetUsersFriends(new RequestOptions());

            foreach (var user in friendsStatusList)
            {
                Console.WriteLine("{0}", user.ScreenName);
            }

            Assert.IsNotEmpty((ICollection)friendsStatusList);
        }
		
		[Test]
		public void Get_UsersFollowers_Test()
		{
			IList<IUser> followersStatusList = twitter.GetUsersFollowers(new RequestOptions());
			
			Assert.IsNotEmpty((ICollection)followersStatusList);
		}
    }
}

