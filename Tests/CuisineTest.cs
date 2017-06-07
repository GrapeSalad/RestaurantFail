using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Restaurants.Objects;

namespace Restaurants
{
  public class CuisineTest : IDisposable
  {
    public CuisineTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=restaurants_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Cuisine.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      Cuisine firstCuisine = new Cuisine("Canadian", "Poutine, eh?");
      Cuisine secondCuisine = new Cuisine("Canadian", "Poutine, eh?");
      Assert.Equal(firstCuisine, secondCuisine);
    }

    [Fact]
    public void Test_Save_ToCuisineDatabase()
    {
      Cuisine testCuisine = new Cuisine("Spanish", "Fukken Paella dude");
      testCuisine.Save();

      List<Cuisine> result = Cuisine.GetAll();
      List<Cuisine> testList = new List<Cuisine>{testCuisine};
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_FindCuisineInDatabase()
    {
      Cuisine testCuisine = new Cuisine("Jamaican-Italian-American Fusion", "Rhasta Pasta");
      testCuisine.Save();

      Cuisine foundCuisine = Cuisine.Find(testCuisine.GetId());

      Assert.Equal(testCuisine, foundCuisine);
    }

    // [Fact]
    // public void Test_GetRestaurants_RetrievesAllRestaurantsWithCuisine()
    // {
    //   Cuisine testCuisine = new Cuisine("Moroccan", "Rasins and nuts and shit oh and tumeric");
    //   testCuisine.Save();

      // Restaurant firstRestaurant = new Restaurant("Lemur", 5, testCuisine.GetId());
      // firstRestaurant.Save();
      // Restaurant secondRestaurant = new Restaurant("Aye-Aye", 4, testCuisine.GetId());
      // secondRestaurant.Save();
      //
      // List<Restaurant> testRestaurantList = new List<Restaurant> {firstRestaurant, secondRestaurant};
      // List<Restaurant> resultRestaurantList = testCuisine.GetRestaurants();
      //
      // Assert.Equal(testRestaurantList, resultRestaurantList);
    // }

    public void Dispose()
    {
      Cuisine.DeleteAll();
    }
  }
}
