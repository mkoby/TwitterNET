using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TwitterNET;

namespace TwitterNET_Tests
{
    [TestFixture]
    public class TwitterNET_Mentions_Tests
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
                    TestStatusID = (long)friendsTimeline.Min(status => status.ID);
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
        { }

        [SetUp]
        public void Test_Setup()
        {
            //twitter = new Twitter("apitest4769", "testaccount");
            twitter = new Twitter("mkoby", "iworld");
        }

        [TearDown]
        public void Test_TearDown()
        {
            twitter = null;
        }

        [Test]
        public void Get_Metions_NoArgs_Test()
        {
            IList<IStatus> mentions = twitter.GetMetions();

            Assert.AreEqual(20, mentions.Count);
        }

        [Test]
        public void Get_Mentions_WithCount()
        {
            int mentionsCount = 10;
            IList<IStatus> mentions = twitter.GetMetions(mentionsCount);

            Assert.AreEqual(10, mentions.Count);
        }

        [Test]
        public void Get_Mentions_WithPageNumber()
        {
            int pageNumber = 2;
            IList<IStatus> mentions = twitter.GetMetions(pageNumber, 20);

            Assert.AreEqual(20, mentions.Count);

            var statusCount = from m in mentions
                              where m.StatusText.Contains("kindle")
                              select m;
        }

    }
}
