using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TwitterNET;

namespace TwitterNET_Tests
{
    [TestFixture]
    public class TwitterNET_Account_Tests
    {
        private Twitter twitter = null;
        private string userName = "apitest4769";
        private string password = "testaccount";

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
        public void VerifyCredentials_Test()
        {
            IUser testUser = twitter.VerifyCredentials(userName, password);

            Assert.IsNotNull(testUser);
            Assert.AreEqual(userName, testUser.ScreenName);
        }
    }
}
