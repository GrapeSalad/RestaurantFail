using Nancy;
using System;
using Restaurants.Objects;
using System.Collections.Generic;
using Nancy.ViewEngines.Razor;

namespace Restaurants
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/cuisines"] = _ => {
        List<Cuisine> allCuisines = Cuisine.GetAll();
        return View["cuisines.cshtml", allCuisines];
      };
      Get["/cuisines/new"] = _ => {
        return View["cuisine_add.cshtml"];
      };
      Post["/cuisines/new"] = _ => {
        Cuisine newCuisine = new Cuisine(Request.Form["cuisine-type"], Request.Form["cuisine-description"]);
        newCuisine.Save();
        return View["success.cshtml"];
      };
      Get["/restaurants"] = _ => {
        List<Restaurant> allRestaurants = Restaurant.GetAll();
        return View["restaurants.cshtml", allRestaurants];
      };
      Get["/restaurants/new"] = _ => {
        List<Cuisine> allCuisines = Cuisine.GetAll();
        return View["restaurant_add.cshtml", allCuisines];
      };
      Post["/restaurants/new"] = _ => {
        Restaurant newRestaurant = new Restaurant(Request.Form["restaurant-name"], Request.Form["restaurant-rating"], Request.Form["cuisine-id"]);
        newRestaurant.Save();
        return View["success.cshtml"];
      };
      Get["/restaurants/new_review"] = _ => {
        List<Restaurant> allRestaurants = Restaurant.GetAll();
        return View["review_add.cshtml", allRestaurants];
      };
      Post["/reviews/new"] = _ => {
        Review newReview = new Review(Request.Form["review-UserName"], Request.Form["review-score"], Request.Form["review-comment"], Request.Form["restaurant-id"]);
        newReview.Save();
        return View["success.cshtml"];
      };
      Get["/cuisines/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var selectedCuisine = Cuisine.Find(parameters.id);
        var cuisineRestaurants = selectedCuisine.GetRestaurants();
        model.Add("cuisine", selectedCuisine);
        model.Add("restaurants", cuisineRestaurants);
        return View["cuisine.cshtml", model];
      };
      Get["/restaurants/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var selectedRestaurant = Restaurant.Find(parameters.id);
        var restaurantReviews = selectedRestaurant.GetReviews();
        var restaurantAvgReviews = Review.GetRestaurantAverage(parameters.id);
        model.Add("restaurant", selectedRestaurant);
        model.Add("reviews", restaurantReviews);
        model.Add("average", restaurantAvgReviews);
        return View["restaurant.cshtml", model];
      };
      Get["cuisines/{id}/edit"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Cuisine selectedCuisine = Cuisine.Find(parameters.id);
        model.Add("selectedCuisine", selectedCuisine);
        return View["cuisine_edit.cshtml", model];
      };
      Patch["/cuisines/{id}/edit"] = parameters => {
        Cuisine selectedCuisine = Cuisine.Find(parameters.id);
        string newType = Request.Form["cuisine-type"];
        string newDescription = Request.Form["cuisine-description"];
        if (newType == "" && newDescription != "")
        {
          selectedCuisine.UpdateDescription(newDescription);
        }
        else if (newType != "" && newDescription == "")
        {
          selectedCuisine.UpdateType(newType);
        }
        else
        {
          selectedCuisine.UpdateDescription(newDescription);
          selectedCuisine.UpdateType(newType);
        }
        return View["success.cshtml"];
      };
      Get["restaurants/{id}/delete"] = parameters => {
        Restaurant selectedRestaurant = Restaurant.Find(parameters.id);
        return View["restaurant_delete.cshtml", selectedRestaurant];
      };
      Delete["restaurants/{id}/delete"] = parameters => {
        Restaurant selectedRestaurant = Restaurant.Find(parameters.id);
        selectedRestaurant.Delete();
        return View["success.cshtml"];
      };
    }
  }
}
