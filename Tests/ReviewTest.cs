using System;
using System.Collections.Generic;
using Xunit;
using System.Data;
using System.Data.SqlClient;
using Restaurants.Objects;

namespace Restaurants
{
  [Collection("Restaurants")]
  public class ReviewTest : IDisposable
  {
    public ReviewTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=restaurants_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Review.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      Review firstReview = new Review("Chimmy Chonga", 4, "IZ VERY GOOD", 1);
      Review secondReview = new Review("Chimmy Chonga", 4, "IZ VERY GOOD", 1);
      Assert.Equal(firstReview, secondReview);
    }

    [Fact]
    public void Test_Save_ToReviewDatabase()
    {
      Review testReview = new Review("Grandma", 1, "No Wheelchair ramps", 1);
      testReview.Save();

      List<Review> result = Review.GetAll();
      List<Review> testList = new List<Review>{testReview};
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_FindReviewInDatabase()
    {
      Review testReview = new Review("Uncle Al", 2, "No Happy Hour", 1);
      testReview.Save();

      Review foundReview = Review.Find(testReview.GetId());

      Assert.Equal(testReview, foundReview);
    }

    public void Dispose()
    {
      Restaurant.DeleteAll();
      Cuisine.DeleteAll();
      Review.DeleteAll();
    }
  }
}
