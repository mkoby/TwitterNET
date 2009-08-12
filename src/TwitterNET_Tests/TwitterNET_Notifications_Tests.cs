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
        { }

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
            RequestOptions requestOptions = new RequestOptions();
            requestOptions.Add(RequestOptionNames.ScreenName, testUserName);
            IUser userFollowed = twitter.TurnDeviceNotificationsOn(requestOptions);

            Assert.IsNotNull(userFollowed);
            Assert.AreEqual(testUserName, userFollowed.ScreenName);
        }

        [Test]
        public void TurnDeviceNotificationsOff_Test()
        {
            RequestOptions requestOptions = new RequestOptions();
            requestOptions.Add(RequestOptionNames.ScreenName, testUserName);
            IUser userFollowed = twitter.TurnDeviceNotificationsOff(requestOptions);

            Assert.IsNotNull(userFollowed);
            Assert.AreEqual(testUserName, userFollowed.ScreenName);
        }

        [Test]
        public void TurnDeviceNotificationsOn_WithUserID_Test()
        {
            RequestOptions requestOptions = new RequestOptions();
            requestOptions.Add(RequestOptionNames.UserID, testUserID);
            IUser userFollowed = twitter.TurnDeviceNotificationsOff(requestOptions);

            Assert.IsNotNull(userFollowed);
            Assert.AreEqual(testUserName, userFollowed.ScreenName);
        }

        [Test]
        public void TurnDeviceNotificationsOff_WithUserID_Test()
        {
            RequestOptions requestOptions = new RequestOptions();
            requestOptions.Add(RequestOptionNames.UserID, testUserID);
            IUser userFollowed = twitter.TurnDeviceNotificationsOff(requestOptions);

            Assert.IsNotNull(userFollowed);
            Assert.AreEqual(testUserName, userFollowed.ScreenName);
        }
    }
}
