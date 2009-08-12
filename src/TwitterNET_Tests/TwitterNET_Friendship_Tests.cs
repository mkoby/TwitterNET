using System;
using System.Collections.Generic;
using NUnit.Framework;
using TwitterNET;

namespace TwitterNET_Tests
{
    [TestFixture]
    public class TwitterNET_Friendship_Tests
    {
        private Twitter twitter = null;
        private const string TestUserName = "mkoby";
        private const string TestFollowUser = "gossipgirl";
        private const long TestUserID = 7263572;

        [TestFixtureSetUp]
        public void TestFixture_Setup()
        {
            twitter = new Twitter("apitest4769", "testaccount");

            //Ensuring the environment is as we want it
            if(twitter.CheckFriendship("apitest4769", TestFollowUser))
            {
                twitter.UnfollowUser(TestFollowUser);
            }

            twitter = null;
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
        public void FriendshipExists_Passing_Test()
        {
            bool friendshipExists = twitter.CheckFriendship("apitest4769", "mkoby");

            Assert.IsTrue(friendshipExists);
        }

        [Test]
        public void FriendshipExists_Failing_Test()
        {
            bool friendshipExists = twitter.CheckFriendship("apitest4769", "LeoLaporte");

            Assert.IsFalse(friendshipExists);
        }

        [Test]
        public void CreateFriendshipWithUserName_Test()
        {
            IUser userToFollow = twitter.FollowUser(TestFollowUser, false);

            Assert.IsNotNull(userToFollow);
            Assert.AreEqual(TestFollowUser, userToFollow.ScreenName);

            //Reset the environment
            twitter.UnfollowUser(TestFollowUser);

            Console.WriteLine("Waiting 5 seconds before next test to help requests not be marked as spam");
            System.Threading.Thread.Sleep(5000);
        }

        [Test]
        public void DeleteFriendshipWithUserName_Test()
        {
            //Set up environment to test
            IUser testFollow = twitter.FollowUser(TestFollowUser, false);
            Assert.IsNotNull(testFollow, "Not following test user. Follow request FAILED.");

            IUser userToUnfollow = twitter.UnfollowUser(TestFollowUser);

            Assert.IsNotNull(userToUnfollow);
            Assert.AreEqual(testFollow.ScreenName, userToUnfollow.ScreenName);

            Console.WriteLine("Waiting 5 seconds before next test to help requests not be marked as spam");
            System.Threading.Thread.Sleep(5000);
        }

        /* 
         * Not sure how to handle the friendships/show
         * API call, it needs a new kind of object but
         * not sure how to structure it.
         * 
         */
        [Test]
        public void GetRelationshipInfo_Test()
        {
            //TODO: Implement Test & Code
            Assert.Ignore("This API method is not implemented at this time");
        }
    }
}
