using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Restaurants.Objects;

namespace Restaurants
{
  [Collection("Restaurants")]
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

    [Fact]
    public void Test_GetRestaurants_RetrievesAllRestaurantsWithCuisine()
    {
      Cuisine testCuisine = new Cuisine("Moroccan", "Rasins and nuts and shit oh and tumeric");
      testCuisine.Save();

      Restaurant firstRestaurant = new Restaurant("Lemur", 5, testCuisine.GetId());
      firstRestaurant.Save();
      Restaurant secondRestaurant = new Restaurant("Aye-Aye", 4, testCuisine.GetId());
      secondRestaurant.Save();

      List<Restaurant> testRestaurantList = new List<Restaurant> {firstRestaurant, secondRestaurant};
      List<Restaurant> resultRestaurantList = testCuisine.GetRestaurants();

      Assert.Equal(testRestaurantList, resultRestaurantList);
    }

    [Fact]
    public void Test_Update_UpdatesCuisineInDatabase()
    {
      //Arrange
      string type = "Mexican";
      string description = "Chimichangas";
      Cuisine testCuisine = new Cuisine(type, description);
      testCuisine.Save();
      string newType = "I dont know";

      //Act
      testCuisine.UpdateType(newType);

      string result = testCuisine.GetType();

      //Assert
      Assert.Equal(newType, result);
    }

    [Fact]
    public void Test_Delete_DeletesCuisineFromDatabase()
    {
      //Arrange
      string type1 = "French";
      string description1 = "BOULLIABAISE";
      Cuisine testCuisine1 = new Cuisine(type1, description1);
      testCuisine1.Save();

      string type2 = "German";
      string description2 = "WEINERSHNITZLE";
      Cuisine testCuisine2 = new Cuisine(type2, description2);
      testCuisine2.Save();

      Restaurant testRestaurant1 = new Restaurant("Le Gummy Bears", 5, testCuisine1.GetId());
      testRestaurant1.Save();
      Restaurant testRestaurant2 = new Restaurant("Nein Germans", 2, testCuisine2.GetId());
      testRestaurant2.Save();

      //Act
      testCuisine1.Delete();
      List<Cuisine> resultCuisines = Cuisine.GetAll();
      List<Cuisine> testCuisineList = new List<Cuisine> {testCuisine2};

      List<Restaurant> resultRestaurants = Restaurant.GetAll();
      List<Restaurant> testRestaurantList = new List<Restaurant> {testRestaurant2};

      //Assert
      Assert.Equal(testCuisineList, resultCuisines);
      Assert.Equal(testRestaurantList, resultRestaurants);
    }

    public void Dispose()
    {
      Cuisine.DeleteAll();
      Restaurant.DeleteAll();
      Review.DeleteAll();
    }
  }
}
