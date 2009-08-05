using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TwitterNET;

namespace TwitterNET_Tests
{
    [TestFixture]
    public class TwitterNET_SocialGraph_Tests
    {
        private Twitter twitter = null;
        private const string TestUserName = "apitest4769";
        private const long TestUserID = 7263572;

        [TestFixtureSetUp]
        public void TestFixture_Setup()
        {
        }

        [TestFixtureTearDown]
        public void TestFixture_TearDown()
        { }

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
        public void GetFollowingList_Test()
        {
            RequestOptions requestOptions = new RequestOptions();
            requestOptions.Add(RequestOptionNames.ScreenName, TestUserName);
            IList<long> friendsList = twitter.GetFollowingList(requestOptions);

            Console.WriteLine("Number of Users Being Followed: {0}", friendsList.Count);
            
            Assert.IsNotNull(friendsList);
            Assert.GreaterOrEqual(friendsList.Count, 20, "Expected at least 1 follower");
        }

        [Test]
        public void GetFollowersList_Test()
        {
            RequestOptions requestOptions = new RequestOptions();
            requestOptions.Add(RequestOptionNames.ScreenName, TestUserName);
            IList<long> friendsList = twitter.GetFollowersList(requestOptions);

            Console.WriteLine("Number of Users Following Specified User: {0}", friendsList.Count);

            Assert.IsNotNull(friendsList);
            Assert.GreaterOrEqual(friendsList.Count, 1, "Expected at least 1 follower");
        }
    }
}
