﻿using System;
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
        private long TestStatusID = long.MinValue;

        [TestFixtureSetUp]
        public void TwitterNET_Tests_Setup()
        {
            //Grab recent status ID
            twitter = new Twitter(String.Empty, String.Empty);

            try
            {
                IList<IStatus> publicTimeline = twitter.GetPublicTimeline();

                if(publicTimeline != null && publicTimeline.Count > 0)
                {
                    foreach (IStatus status in publicTimeline)
                    {
                        if (TestStatusID == long.MinValue || TestStatusID > status.ID)
                            TestStatusID = status.ID;
                    }
                }

                //ensuring we have a status that when tested will have tweets bothe before and after
                Random rnd = new Random(DateTime.Now.Millisecond);
                TestStatusID = TestStatusID - rnd.Next(10000, 250000);
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
			RequestOptions myOptions = new RequestOptions();
			
            IList<IStatus> statusList = twitter.GetFriendsTimeline(myOptions);
            Console.WriteLine("Status Count: {0}", statusList.Count);

            Assert.IsNotEmpty((ICollection)statusList, "Status list was empty, expected at least 1 status returned");
        }

        [Test]
        public void GetFriendsTimeline_SinceStatusID_Test()
        {
			RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.SinceID, TestStatusID);
			
            IList<IStatus> statusList = twitter.GetFriendsTimeline(requestOptions);

            //Make sure we got at least 1 status back
            Assert.IsNotEmpty((ICollection)statusList, "Status list was empty, expected at least 1 status returned");

            //Test to ensure statusID falls where we think it should
            long minStatusID = statusList.Min(status => status.ID);

            Console.WriteLine("TestStatusID: {0}\nMinStatusID: {1}", TestStatusID, minStatusID);
            Assert.Greater(minStatusID, TestStatusID);
        }

        [Test]
        public void GetFriendsTimeline_MaxStatusID_Test()
        {
            RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.MaxID, TestStatusID);
			
            IList<IStatus> statusList = twitter.GetFriendsTimeline(requestOptions);

            //Make sure we got at least 1 status back
            Assert.IsNotEmpty((ICollection)statusList, "Status list was empty, expected at least 1 status returned");

            //Test to ensure statusID falls where we think it should
            long maxStatusID = statusList.Max(status => status.ID);
            
            Console.WriteLine("TestStatusID: {0}\nMaxStatusID: {1}", TestStatusID, maxStatusID);
            Assert.Less(maxStatusID, TestStatusID, "MaxStatusID is GREATER than TestStatusID");
        }

        [Test]
        public void GetFriendsTimeline_Page_Test()
        {
            //TODO: Find better method to test than just pull page 2 of tweets
            RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.Page, 2);
			requestOptions.Add(RequestOptionNames.Count, 10);
			
            IList<IStatus> statusList = twitter.GetFriendsTimeline(requestOptions);

            #region Console

            Console.WriteLine("Statuses from page 2");

            foreach (IStatus status in statusList)
            {
                Console.WriteLine("({0}){1}: {2}", status.ID, status.User.ScreenName, status.StatusText);
            }

            #endregion

            Assert.IsNotEmpty((ICollection)statusList, "Status list was empty, expected at least 1 status returned");
        }

        [Test]
        public void GetFriendsTimeline_ReturnCountOnly_Test()
        {
            RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.Count, 40);
			
            IList<IStatus> statusList = twitter.GetFriendsTimeline(requestOptions);

            //Make sure we got at least 1 status back
            Assert.IsNotEmpty((ICollection)statusList, "Status list was empty, expected at least 1 status returned");

            #region Console

            //Console printout to compare with online webpage to ensure accuracy in order and recent tweets
            foreach (IStatus status in statusList)
            {
                Console.WriteLine("({0}){1}: {2}", status.ID, status.User.ScreenName, status.StatusText);
            }

            #endregion

            Assert.AreEqual(statusList.Count, 40, "Expected 40 statuses only had " + statusList.Count);
        }
    }
}
