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
                IList<IStatus> friendsTimeline = twitter.GetFriendsTimeline();

                if (friendsTimeline != null && friendsTimeline.Count > 0)
                {
                    TestStatusID = (long) friendsTimeline.Min(status => status.ID);
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
            IList<IStatus> statusList = twitter.GetUserTimeline(TestUserName);

            foreach (var status in statusList)
            {
                Console.WriteLine("{0}: {1}", status.User.ScreenName, status.StatusText);
            }

            Assert.AreEqual(20, statusList.Count);
        }

        [Test]
        public void Get_UserTimeline_longID()
        {
            IList<IStatus> statusList = twitter.GetUserTimeline(TestUserID);

            foreach (var status in statusList)
            {
                Console.WriteLine("{0}: {1}", status.User.ScreenName, status.StatusText);
            }

            Assert.AreEqual(20, statusList.Count);
            Assert.AreEqual(TestUserID, statusList[0].User.ID);
        }
        
        [Test]
        public void Get_UserTimeline_SinceStatusID()
        {
            IList<IStatus> statusList = twitter.GetUserTimeline(TestUserName, TestStatusID, false);

            //Make sure we got at least 1 status back
            Assert.IsNotEmpty((ICollection)statusList, "Status list was empty, expected at least 1 status returned");

            long returnedStatusID = (long)statusList.Min(status => status.ID);

            Console.WriteLine("TestStatusID: {0}\nMinStatusID: {1}", TestStatusID, returnedStatusID);
            Assert.Greater(returnedStatusID, TestStatusID);
        }

        [Test]
        public void Get_UserTimeline_MaxStatusID()
        {
            IList<IStatus> statusList = twitter.GetUserTimeline(TestUserName, TestStatusID, true);

            //Make sure we got at least 1 status back
            Assert.IsNotEmpty((ICollection)statusList, "Status list was empty, expected at least 1 status returned");

            long returnedStatusID = (long)statusList.Max(status => status.ID);

            Console.WriteLine("TestStatusID: {0}\nMaxStatusID: {1}", TestStatusID, returnedStatusID);
            Assert.Less(returnedStatusID, TestStatusID);
        }
    }
}

