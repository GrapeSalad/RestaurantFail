using System;
using System.Collections.Generic;
using Xunit;
using System.Data;
using System.Data.SqlClient;
using Restaurants.Objects;

namespace Restaurants
{
  [Collection("Restaurants")]
  public class RestaurantTest : IDisposable
  {
    public RestaurantTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=restaurants_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Restaurant.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      Restaurant firstRestaurant = new Restaurant("Chimmy Chonga", 4, 1);
      Restaurant secondRestaurant = new Restaurant("Chimmy Chonga", 4, 1);
      Assert.Equal(firstRestaurant, secondRestaurant);
    }

    [Fact]
    public void Test_Save_ToRestaurantDatabase()
    {
      Restaurant testRestaurant = new Restaurant("Grandma's House", 5, 1);
      testRestaurant.Save();

      List<Restaurant> result = Restaurant.GetAll();
      List<Restaurant> testList = new List<Restaurant>{testRestaurant};
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_FindRestaurantInDatabase()
    {
      Restaurant testRestaurant = new Restaurant("j", 3, 1);
      testRestaurant.Save();

      Restaurant foundRestaurant = Restaurant.Find(testRestaurant.GetId());

      Assert.Equal(testRestaurant, foundRestaurant);
    }

    public void Dispose()
    {
      Restaurant.DeleteAll();
      Cuisine.DeleteAll();
      Review.DeleteAll();
    }
  }
}
