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

        [TestFixtureSetUp]
        public void TwitterNET_Tests_Setup()
        {}

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
            IList<IStatus> statusList = twitter.GetPublicTimeline();

            #region Console

            foreach (IStatus status in statusList)
            {
                Console.WriteLine("{0}: {1}", status.User.ScreenName, status.StatusText);
            }

            #endregion

            Assert.AreEqual(20, statusList.Count);
        }

        [Test]
        public void TwitterNET_GetSingleStatus_Test()
        {
            IStatus status = twitter.GetSingleStatus(TestStatusID);

            Assert.IsNotNull(status); //Check to make sure we actually got something back

            Assert.AreEqual(TestStatusID, status.ID); //Assure we got the status we were expecting

            Assert.AreEqual("Looks like Sun just pulled a Yahoo", status.StatusText); //Assure we got the right status text

        }

        [Test]
        public void TwitterNET_UpdateStatus_Test()
        {
            string dateTimeString = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            string TestStatus = "This is a test status update sent at " + dateTimeString;
            IStatus updatedStatus = twitter.UpdateStatus(TestStatus);

            Assert.IsNotNull(updatedStatus);
            Assert.AreEqual(TestStatus, updatedStatus.StatusText);
        }
		
		[Test]
		public void TwitterNET_DeleteStatus_Test()
		{
			IStatus myStatus = twitter.UpdateStatus("This is a Test Status that I will Delete");
			
			Assert.IsNotNull(myStatus, "Test can not continue due to no status being returned");
			
			IStatus deletedStatus = twitter.DeleteStatus(myStatus.ID);
			myStatus = twitter.GetSingleStatus(myStatus.ID);
			
			Assert.IsNotNull(deletedStatus);
		}
    }
}
