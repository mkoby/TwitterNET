using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;
using TwitterNET;

namespace TwitterNET_Tests
{
	[TestFixture]
	public class TwitterNET_Favorites_Tests
	{
		private Twitter twitter = null;
		private long idToFavorite = 1807262311;
		
		[TestFixtureSetUp]
        public void TwitterNET_Tests_Setup()
        {
			try
			{
				//Make sure the status we're going to 
				//test with is NOT currently a favorite.
				twitter = new Twitter("apitest4769", "testaccount");
				twitter.DeleteFavorite(idToFavorite);
			}
			catch(Exception ex)
			{
				Console.WriteLine("{0}\n\n{1}", ex.Message, ex.StackTrace);
			}
			finally
			{
				twitter = null;
			}
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
		public void GetFavorites_Test()
		{
			IList<IStatus> favoritesList = twitter.GetFavorites(new RequestOptions());
			
			Assert.IsNotNull((ICollection)favoritesList);
			Assert.AreEqual(3, favoritesList.Count);			
		}
		
		[Test]
		public void CreateFavorite_Test()
		{
			IList<IStatus> currentFavorites = twitter.GetFavorites(new RequestOptions());
			int startingCount = currentFavorites.Count;
			IStatus favoriteStatus = twitter.FavoriteStatus(idToFavorite);
			
			Assert.IsNotNull(favoriteStatus);			
			Assert.IsTrue(favoriteStatus.StatusText.StartsWith("breathalyser"));
			
			currentFavorites = twitter.GetFavorites(new RequestOptions());
			Assert.Greater(currentFavorites.Count, startingCount);
			
			//Delete Favorite
			twitter.DeleteFavorite(idToFavorite);
		}
		
		[Test]
		public void DeleteFavorite_Test()
		{
			//Ensure that the favorite we want to delete is in fact a favorite
			IStatus createdFavorite = twitter.FavoriteStatus(idToFavorite);
			Assert.IsNotNull(createdFavorite);
			
			IList<IStatus> currentFavorites = twitter.GetFavorites(new RequestOptions());
			int startingCount = currentFavorites.Count;
			IStatus deletedFavorite = twitter.DeleteFavorite(idToFavorite);
			
			Assert.IsNotNull(deletedFavorite);
			Assert.IsTrue(deletedFavorite.StatusText.StartsWith("breathalyser"));
			
			currentFavorites = twitter.GetFavorites(new RequestOptions());
			Assert.Less(currentFavorites.Count, startingCount);
		}
	}
}
