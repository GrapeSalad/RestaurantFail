using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Restaurants;

namespace Restaurants.Objects
{
  public class Restaurant
  {
    private int _id;
    private string _name;
    private int _rating;
    private int _cuisine_id;

    public Restaurant(string Name, int Rating, int Cuisine_Id, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _rating = Rating;
      _cuisine_id = Cuisine_Id;
    }
    public override bool Equals(System.Object otherRestaurant)
    {
      if(!(otherRestaurant is Restaurant))
      {
        return false;
      }
      else
      {
        Restaurant newRestaurant = (Restaurant) otherRestaurant;
        bool nameEquality = (this.GetName() == newRestaurant.GetName());
        bool ratingEquality = (this.GetRating() == newRestaurant.GetRating());
        bool cuisineIdEquality = (this.GetCuisineId() == newRestaurant.GetCuisineId());
        return (nameEquality && ratingEquality && cuisineIdEquality);
      }
    }
    public int GetId()
    {
      return _id;
    }
    public int GetCuisineId()
    {
      return _cuisine_id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public int GetRating()
    {
      return _rating;
    }
    public void SetRating(int newRating)
    {
      _rating = newRating;
    }

    public static List<Restaurant> GetAll()
    {
      List<Restaurant> allRestaurants = new List<Restaurant>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int restaurantId = rdr.GetInt32(0);
        string restaurantName = rdr.GetString(1);
        int restaurantRating = rdr.GetInt32(2);
        int cuisineId = rdr.GetInt32(3);
        Restaurant newRestaurant = new Restaurant(restaurantName, restaurantRating, cuisineId, restaurantId);
        allRestaurants.Add(newRestaurant);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allRestaurants;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO restaurants (name, rating, cuisine_id) OUTPUT INSERTED.id VALUES (@RestaurantName, @RestaurantRating, @CuisineId)", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@RestaurantName";
      nameParameter.Value = this.GetName();
      SqlParameter ratingParameter = new SqlParameter();
      ratingParameter.ParameterName = "@RestaurantRating";
      ratingParameter.Value = this.GetRating();
      SqlParameter cuisineIdParameter = new SqlParameter();
      cuisineIdParameter.ParameterName = "@CuisineId";
      cuisineIdParameter.Value = this.GetCuisineId();
      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(ratingParameter);
      cmd.Parameters.Add(cuisineIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM restaurants;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
