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
      Get["/reviews"] = _ => {
        List<Review> allReviews = Review.GetAll();
        return View["reviews.cshtml", allReviews];
      };
      Get["/reviews/new"] = _ => {
        List<Restaurant> allRestaurants = Restaurant.GetAll();
        return View["review_add.cshtml", allRestaurants];
      };
      Post["/reviews/new"] = _ => {
        Review newReview = new Review(Request.Form["review-UserName"], Request.Form["review-score"], Request.Form["review-comment"], Request.Form["restaurant-id"]);
        newReview.Save();
        return View["success.cshtml"];
      };
    }
  }
}
