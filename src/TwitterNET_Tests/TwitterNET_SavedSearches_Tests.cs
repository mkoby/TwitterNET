using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TwitterNET;

namespace TwitterNET_Tests
{
    [TestFixture]
    public class TwitterNET_SavedSearches_Tests
    {
        private Twitter twitter = null;
        private long testIDtoPull = 2132193;
        private string testSavedSearchQuery = "Android";
        private string testSaveSearchToDelete = "Sogeti";
        private long testId = 0;


        [TestFixtureSetUp]
        public void TestFixture_Setup()
        {
            twitter = new Twitter("apitest4769", "testaccount");
            IList<SavedSearch> savedSearchs = twitter.GetSavedSearches();

            //Make sure we have the ID of the saved search to test delete
            //We either have it left over from a failing test or
            //we will create it from scratch
            if (savedSearchs != null && savedSearchs.Count > 0)
            {
                var saveds = from s in savedSearchs
                             where s.Query.Equals(testSaveSearchToDelete)
                             select s;

                if (saveds.Count() > 0)
                {
                    testId = saveds.First().Id;
                }
                else
                {
                    SavedSearch ss = twitter.CreateSavedSearch(testSaveSearchToDelete);
                    testId = ss.Id;
                }

                saveds = from s in savedSearchs
                             where s.Query.Equals(testSavedSearchQuery)
                             select s;

                if (saveds.Count() > 0)
                    twitter.DeleteSavedSearch(saveds.FirstOrDefault().Id);
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
        public void GetSavedSearches_Test()
        {
            IList<SavedSearch> savedSearches = twitter.GetSavedSearches();

            Assert.IsNotNull(savedSearches);
            Assert.Greater(savedSearches.Count, 1);
        }

        [Test]
        public void ShowSavedSearch_Test()
        {
            SavedSearch savedSearch = twitter.ShowSavedSearch(testIDtoPull);

            Assert.IsNotNull(savedSearch);
        }

        [Test]
        public void CreateSavedSearch_Test()
        {
            SavedSearch mySavedSearch = twitter.CreateSavedSearch(testSavedSearchQuery);

            Assert.IsNotNull(mySavedSearch);
            Assert.AreEqual(testSavedSearchQuery, mySavedSearch.Query);

            mySavedSearch = twitter.DeleteSavedSearch(mySavedSearch.Id);
            Assert.IsNotNull(mySavedSearch);
        }

        [Test]
        public void DeleteSavedSearch_Test()
        {
            SavedSearch mySavedSearch = twitter.DeleteSavedSearch(testId);

            Assert.IsNotNull(mySavedSearch);
            Assert.AreEqual(testSaveSearchToDelete, mySavedSearch.Query);
        }
    }

}