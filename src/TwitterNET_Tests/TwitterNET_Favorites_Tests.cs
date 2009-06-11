using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

			    IList<StatusMessage> favorites = twitter.GetFavorites(new RequestOptions());

			    var status = from f in favorites
			                 where f.ID.Equals(idToFavorite)
			                 select f;

                if (status != null && status.Count() > 0)
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
		public void GetFavorites_Test_Should_Return_List_of_Current_Favorites()
		{
			IList<StatusMessage> favoritesList = twitter.GetFavorites(new RequestOptions());
			
			Assert.IsNotNull((ICollection)favoritesList);
			Assert.Greater(favoritesList.Count, 0, "No favorites returned");			
		}
		
		[Test]
		public void CreateFavorite_Test_Should_Create_a_New_Favorite_Out_Of_Test_Status()
		{
			StatusMessage favoriteStatus = twitter.FavoriteStatus(idToFavorite);

            //Delete Favorite to clean up
            twitter.DeleteFavorite(idToFavorite);
			
			Assert.IsNotNull(favoriteStatus);			
			Assert.AreEqual(idToFavorite, favoriteStatus.ID);
		}
		
		[Test]
		public void DeleteFavorite_Test_Should_Delete_Test_Favorite()
		{
			//Ensure that the favorite we want to delete is in fact a favorite
			StatusMessage createdFavorite = twitter.FavoriteStatus(idToFavorite);
			Assert.IsNotNull(createdFavorite);
			
			StatusMessage deletedFavorite = twitter.DeleteFavorite(idToFavorite);
			
			Assert.IsNotNull(deletedFavorite);
			Assert.AreEqual(idToFavorite, deletedFavorite.ID);
		}
	}
}
