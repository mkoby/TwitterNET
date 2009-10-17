using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TwitterNET;

namespace TwitterNET_Tests
{
    [TestFixture]
    public class TwitterNET_Notifications_Tests
    {
        private Twitter twitter = null;
        private string testUserName = "mkoby";
        private long testUserID = 7263572;

        [TestFixtureSetUp]
        public void TwitterNET_Tests_Setup()
        {
            twitter = new Twitter("apitest4769", "testaccount");

            //Ensure we're following the user we're testing device
            //notifications on
            if (!twitter.CheckFriendship("apitest4769", testUserName))
                twitter.FollowUser(testUserName, false);
        }

        [TestFixtureTearDown]
        public void TwitterNET_Tests_TearDown()
        { }

        [SetUp]
        public void Test_Setup()
        {
            twitter = new Twitter("apitest4769", "testaccount");
        }

        [TearDown]
        public void Test_TearDown()
        {
            Console.WriteLine("Waiting 5 seconds before next test to help requests not be marked as spam");
            System.Threading.Thread.Sleep(5000);
        }

        [Test]
        public void TurnDeviceNotificationsOn_Test()
        {
            StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
            statusRequestOptions.Add(StatusRequestOptionNames.ScreenName, testUserName);
            IUser userFollowed = twitter.TurnDeviceNotificationsOn(statusRequestOptions);

            Assert.IsNotNull(userFollowed);
            Assert.AreEqual(testUserName, userFollowed.ScreenName);
        }

        [Test]
        public void TurnDeviceNotificationsOff_Test()
        {
            StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
            statusRequestOptions.Add(StatusRequestOptionNames.ScreenName, testUserName);
            IUser userFollowed = twitter.TurnDeviceNotificationsOff(statusRequestOptions);

            Assert.IsNotNull(userFollowed);
            Assert.AreEqual(testUserName, userFollowed.ScreenName);
        }

        [Test]
        public void TurnDeviceNotificationsOn_WithUserID_Test()
        {
            StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
            statusRequestOptions.Add(StatusRequestOptionNames.UserID, testUserID);
            IUser userFollowed = twitter.TurnDeviceNotificationsOff(statusRequestOptions);

            Assert.IsNotNull(userFollowed);
            Assert.AreEqual(testUserName, userFollowed.ScreenName);
        }

        [Test]
        public void TurnDeviceNotificationsOff_WithUserID_Test()
        {
            StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
            statusRequestOptions.Add(StatusRequestOptionNames.UserID, testUserID);
            IUser userFollowed = twitter.TurnDeviceNotificationsOff(statusRequestOptions);

            Assert.IsNotNull(userFollowed);
            Assert.AreEqual(testUserName, userFollowed.ScreenName);
        }
    }
}
