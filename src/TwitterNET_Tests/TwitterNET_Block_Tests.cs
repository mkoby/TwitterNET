using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TwitterNET;

namespace TwitterNET_Tests
{
    [TestFixture]
    public class TwitterNET_Block_Tests
    {
        private Twitter twitter = null;
        private const string TestUserName = "mkoby";
        private const long TestUserID = 7263572;
        private const long TestUserAlwaysBlocked = 71876190;

        [TestFixtureSetUp]
        public void TestFixture_Setup()
        {
            try
            {
                twitter = new Twitter("apitest4769", "testaccount");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            twitter = null;
        }

        [TestFixtureTearDown]
        public void TestFixture_TearDown()
        {
            twitter = new Twitter("apitest4769", "testaccount");

            if (!twitter.CheckFriendship("apitest4769", TestUserName))
                twitter.FollowUser(TestUserName, false);

            twitter = null;
        }

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
        public void BlockUser_Test()
        {
            IUser blockedUser = twitter.BlockUser(TestUserID);

            Assert.IsNotNull(blockedUser);
            Assert.AreEqual(TestUserName, blockedUser.ScreenName);
        }

        [Test]
        public void UnblockUser_Test()
        {
            IUser unblockedUser = twitter.UnblockUser(TestUserID);

            Assert.IsNotNull(unblockedUser);
            Assert.AreEqual(TestUserName, unblockedUser.ScreenName);
        }

        [Test]
        public void IsBlocked_Test()
        {
            bool isBlocked = twitter.IsBlocked(TestUserID);

            Assert.IsFalse(isBlocked);

            isBlocked = twitter.IsBlocked(TestUserAlwaysBlocked);

            Assert.IsTrue(isBlocked);
        }

        [Test]
        public void GetBlockedUsers_Test()
        {
            IList<IUser> blockedUsers = twitter.GetBlockedUsers();

            Assert.IsNotNull(blockedUsers);
            Assert.AreEqual(1, blockedUsers.Count);
        }

        [Test]
        public void GetBlockedUserIds_Test()
        {
            IList<long> blockedUserIds = twitter.GetBlockedUsersIds();

            Assert.IsNotNull(blockedUserIds);
            Assert.AreEqual(1, blockedUserIds.Count);
        }
    }
}
