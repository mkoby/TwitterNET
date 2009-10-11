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

        private string defaultBackgroundColor = "9ae4e8";
        private string defaultTextColor = "000000";
        private string defaultLinkColor = "0000ff";
        private string defaultSidebarFillColor = "e0ff92";
        private string defaultSidebarBorderColor = "87bc44";

        private string newColor = "ff0000";

        private string defaultDescription = "API Library Test Account";
        private string defaultLocation = "GitHub/TwitterNET";
        private string defaultWebsite = "http://wiki.github.com/mkoby/TwitterNET";

        private string newProfileInfo = "Testing";

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

        [Test]
        public void RateLimitStatus_Test()
        {
            IDictionary<string, object> RateLimitHash = twitter.RateLimitStatus();

            Assert.IsNotNull(RateLimitHash);

            int rateHourlyLimit = Convert.ToInt32(RateLimitHash["hourly-limit"].ToString());

            Assert.AreEqual(150, rateHourlyLimit);
        }

        [Test]
        public void EndSession_Test()
        {
            IDictionary<string, object> endSessionHash = twitter.EndTwitterSession();

            Assert.IsNotNull(endSessionHash);

            string logoutMsg = endSessionHash["error"].ToString().ToLower();

            Assert.AreEqual("logged out.", logoutMsg);
        }

        [Test]
        public void UpdateDeliveryDevice_Test()
        {
            IUser user = twitter.UpdateDeliveryDevice(Twitter.DeliveryDeviceType.None);

            Assert.IsNotNull(user);
            Assert.AreEqual("apitest4769", user.ScreenName);
        }

        [Test]
        public void UpdateProfileColors_Test()
        {
            AccountRequestOptions requestOptions = new AccountRequestOptions();
            requestOptions.Add(AccountRequestOptionNames.ProfileBackgroundColor, newColor);
            requestOptions.Add(AccountRequestOptionNames.ProfileLinkColor, newColor);
            requestOptions.Add(AccountRequestOptionNames.ProfileSidebarBorderColor, newColor);
            requestOptions.Add(AccountRequestOptionNames.ProfileSidebarFillColor, newColor);
            requestOptions.Add(AccountRequestOptionNames.ProfileTextColor, newColor);

            IUser user = twitter.UpdateProfileColors(requestOptions);

            Assert.IsNotNull(user);
            Assert.AreEqual(newColor, user.ProfileBackgroundColor);
            Assert.AreEqual(newColor, user.ProfileLinkColor);
            Assert.AreEqual(newColor, user.ProfileSidebarBorderColor);
            Assert.AreEqual(newColor, user.ProfileSidebarFillColor);
            Assert.AreEqual(newColor, user.ProfileTextColor);

            //Reset environment
            requestOptions = new AccountRequestOptions();
            requestOptions.Add(AccountRequestOptionNames.ProfileBackgroundColor, defaultBackgroundColor);
            requestOptions.Add(AccountRequestOptionNames.ProfileLinkColor, defaultLinkColor);
            requestOptions.Add(AccountRequestOptionNames.ProfileSidebarBorderColor, defaultSidebarBorderColor);
            requestOptions.Add(AccountRequestOptionNames.ProfileSidebarFillColor, defaultSidebarFillColor);
            requestOptions.Add(AccountRequestOptionNames.ProfileTextColor, defaultTextColor);

            Assert.IsNotNull(twitter.UpdateProfileColors(requestOptions));
            
        }

        [Test]
        public void UpdateProfileImage_Test()
        {
            Assert.Ignore("Ignoring this due to lack of understanding on how to properly upload photos");
        }

        [Test]
        public void UpdateProfileBackgroundImage_Test()
        {
            Assert.Ignore("Ignoring this due to lack of understanding on how to properly upload photos");
        }

        [Test]
        public void UpdateProfileInfo_Test()
        {
            AccountRequestOptions requestOptions = new AccountRequestOptions();
            requestOptions.Add(AccountRequestOptionNames.Description, newProfileInfo);
            //Twitter automatically puts "http://" in front of website so, accounting for that
            requestOptions.Add(AccountRequestOptionNames.Website, "http://" + newProfileInfo);
            requestOptions.Add(AccountRequestOptionNames.Location, newProfileInfo);
            IUser user = twitter.UpdateProfileInfo(requestOptions);

            Assert.IsNotNull(user);
            Assert.AreEqual(newProfileInfo, user.Description);
            //Again, Twitter automatically puts "http://" in front of website so, accounting for that
            Assert.AreEqual("http://" + newProfileInfo, user.Website);
            Assert.AreEqual(newProfileInfo, user.Location);

            //Reset environment
            requestOptions = new AccountRequestOptions();
            requestOptions.Add(AccountRequestOptionNames.Description, defaultDescription);
            requestOptions.Add(AccountRequestOptionNames.Website, defaultWebsite);
            requestOptions.Add(AccountRequestOptionNames.Location, defaultLocation);

            Assert.IsNotNull(twitter.UpdateProfileInfo(requestOptions));
        }

    }
}
