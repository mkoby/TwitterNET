using System;
using System.Collections.Generic;

using NUnit.Framework;
using TwitterNET;

namespace TwitterNET_Tests
{
    [TestFixture]
    public class TwitterNET_Tests
    {
        private Twitter twitter = null;
        private long TestStatusID = 1460203541;
        private long retweetStatusId = long.MinValue;

        [TestFixtureSetUp]
        public void TwitterNET_Tests_Setup()
        {
            twitter = new Twitter();
            IList<StatusMessage> publicTimeline = twitter.GetPublicTimeline();

            if (publicTimeline != null && publicTimeline.Count > 0)
                retweetStatusId = publicTimeline[0].ID;
        }

        [TestFixtureTearDown]
        public void TwitterNET_Tests_TearDown()
        {}

        [SetUp]
        public void Test_Setup()
        {
            twitter = new Twitter("apitest4769", "testaccount");
        }

        [TearDown]
        public void Test_TearDown()
        {}

        [Test]
        public void TwitterNET_GetPublicTimeline_Test()
        {
            IList<StatusMessage> statusList = twitter.GetPublicTimeline();
            Assert.IsNotNull(statusList);

            #region Console

            foreach (StatusMessage status in statusList)
            {
                Console.WriteLine("{0}: {1}", status.Author.ScreenName, status.MessageText);
            }

            #endregion

            Assert.AreEqual(20, statusList.Count);
        }

        [Test]
        public void TwitterNET_GetSingleStatus_Test()
        {
            StatusMessage status = twitter.GetSingleStatus(TestStatusID);

            Assert.IsNotNull(status); //Check to make sure we actually got something back

            Assert.AreEqual(TestStatusID, status.ID); //Assure we got the status we were expecting

            Assert.AreEqual("Looks like Sun just pulled a Yahoo", status.MessageText); //Assure we got the right status text

        }

        [Test]
        public void TwitterNET_UpdateStatus_Test()
        {
            string dateTimeString = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            string TestStatus = "This is a test status update sent at " + dateTimeString;
            StatusMessage updatedStatus = twitter.UpdateStatus(TestStatus);

            Assert.IsNotNull(updatedStatus);
            Assert.AreEqual(TestStatus, updatedStatus.MessageText);
        }
		
		[Test]
		public void TwitterNET_DeleteStatus_Test()
		{
			StatusMessage myStatus = twitter.UpdateStatus("This is a Test Status that I will Delete");
			
			Assert.IsNotNull(myStatus, "Test can not continue due to no status being returned");
			
			StatusMessage deletedStatus = twitter.DeleteStatus(myStatus.ID);
			
			Assert.IsNotNull(deletedStatus);
		}

        [Test]
        public void GetSingleUser_Test()
        {
            StatusRequestOptions requestOptions = new StatusRequestOptions();
            requestOptions.Add(StatusRequestOptionNames.ScreenName, "mkoby");
            IUser user = twitter.GetSingleUser(requestOptions);

            Assert.IsNotNull(user);
            Assert.AreEqual("mkoby", user.ScreenName);
        }

        [Test]
        public void RetweetStatus_Test()
        {
            StatusMessage retweetedStatus = twitter.RetweetStatus(retweetStatusId);

            Assert.IsNotNull(retweetedStatus);
        }
    }
}
