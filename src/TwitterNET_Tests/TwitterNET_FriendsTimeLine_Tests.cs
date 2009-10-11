using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TwitterNET;

namespace TwitterNET_Tests
{
    [TestFixture]
    public class TwitterNET_FriendsTimeLine_Tests
    {
        private Twitter twitter = null;
        private long minTestStatusID = long.MinValue,
					 maxTestStatusID = long.MinValue;

        [TestFixtureSetUp]
        public void TwitterNET_Tests_Setup()
        {
            //Grab recent status ID
            twitter = new Twitter(String.Empty, String.Empty);

            try
            {
                StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
                statusRequestOptions.Add(StatusRequestOptionNames.ScreenName, "apitest4769");
                IList<StatusMessage> userTimeline = twitter.GetUserTimeline(statusRequestOptions);

                if(userTimeline != null && userTimeline.Count > 0)
                {
					//Random rnd = new Random(DateTime.Now.Millisecond);
					//int rndNum = rnd.Next(100000, 250000);
                    minTestStatusID = userTimeline.Min(status => status.ID);
					maxTestStatusID = userTimeline.Max(status => status.ID);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        [TestFixtureTearDown]
        public void TwitterNET_Tests_TearDown()
        {
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
        public void GetFriendsTimeline_NoArgs_Test()
        {
			StatusRequestOptions myOptions = new StatusRequestOptions();
			
            IList<StatusMessage> statusList = twitter.GetFriendsTimeline(myOptions);
            Console.WriteLine("Status Count: {0}", statusList.Count);

            Assert.IsNotEmpty((ICollection)statusList, 
			                  "Status list was empty, expected at least 1 status returned");
        }

        [Test]
        public void GetFriendsTimeline_SinceStatusID_Test()
        {
            Assert.AreNotEqual(long.MinValue, minTestStatusID, "minTestStatusID is Long.MinValue");

			StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.SinceID, minTestStatusID);
			
            IList<StatusMessage> statusList = twitter.GetFriendsTimeline(statusRequestOptions);

            //Make sure we got at least 1 status back
            Assert.IsNotEmpty((ICollection)statusList, 
			                  "Status list was empty, expected at least 1 status returned");

            //Test to ensure statusID falls where we think it should
            long minStatusID = statusList.Min(status => status.ID);

            Console.WriteLine("TestStatusID: {0}\nMinStatusID: {1}", minTestStatusID, minStatusID);
            Assert.GreaterOrEqual(minStatusID, minTestStatusID);
        }

        [Test]
        public void GetFriendsTimeline_MaxStatusID_Test()
        {
            Assert.AreNotEqual(long.MinValue, maxTestStatusID, "maxTestStatusID is Long.MinValue");

            StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.MaxID, maxTestStatusID);
			
            IList<StatusMessage> statusList = twitter.GetFriendsTimeline(statusRequestOptions);

            //Make sure we got at least 1 status back
            Assert.IsNotEmpty((ICollection)statusList, 
			                  "Status list was empty, expected at least 1 status returned");

            //Test to ensure statusID falls where we think it should
            long maxStatusID = statusList.Max(status => status.ID);
            
            Console.WriteLine("TestStatusID: {0}\nMaxStatusID: {1}", maxTestStatusID, maxStatusID);
            Assert.LessOrEqual(maxStatusID, maxTestStatusID, "MaxStatusID is GREATER than TestStatusID");
        }

        [Test]
        public void GetFriendsTimeline_Page_Test()
        {
            //TODO: Find better method to test than just pull page 2 of tweets
            StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.Page, 2);
			statusRequestOptions.Add(StatusRequestOptionNames.Count, 7);
			
            IList<StatusMessage> statusList = twitter.GetFriendsTimeline(statusRequestOptions);

            #region Console

            Console.WriteLine("Statuses from page 2");

            foreach (StatusMessage status in statusList)
            {
                Console.WriteLine("({0}){1}: {2}", status.ID, status.Author.ScreenName, status.MessageText);
            }

            #endregion

            Assert.IsNotEmpty((ICollection)statusList, 
			                  "Status list was empty, expected at least 1 status returned");
            Assert.AreEqual(7, statusList.Count);
        }

        [Test]
        public void GetFriendsTimeline_ReturnCountOnly_Test()
        {
            StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.Count, 5);
			
            IList<StatusMessage> statusList = twitter.GetFriendsTimeline(statusRequestOptions);

            //Make sure we got at least 1 status back
            Assert.IsNotEmpty((ICollection)statusList, 
			                  "Status list was empty, expected at least 1 status returned");

            #region Console

            //Console printout to compare with online webpage to ensure accuracy in order and recent tweets
            foreach (StatusMessage status in statusList)
            {
                Console.WriteLine("({0}){1}: {2}", status.ID, status.Author.ScreenName, status.MessageText);
            }

            #endregion

            Assert.AreEqual(5, statusList.Count,  
			                "Expected 5 statuses only had " + statusList.Count);
        }
    }
}
