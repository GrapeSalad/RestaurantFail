using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Restaurants;

namespace Restaurants.Objects
{
  public class Review
  {
    private int _id;
    private string _username;
    private int _score;
    private string _comment;
    private int _restaurant_id;

    public Review(string UserName, int Score, string Comment, int Restaurant_Id, int Id = 0)
    {
      _id = Id;
      _username = UserName;
      _score = Score;
      _comment = Comment;
      _restaurant_id = Restaurant_Id;
    }
    public override bool Equals(System.Object otherReview)
    {
      if(!(otherReview is Review))
      {
        return false;
      }
      else
      {
        Review newReview = (Review) otherReview;
        bool usernameEquality = (this.GetUserName() == newReview.GetUserName());
        bool scoreEquality = (this.GetScore() == newReview.GetScore());
        bool commentEquality = (this.GetComment() == newReview.GetComment());
        bool restaurantIdEquality = (this.GetRestaurantId() == newReview.GetRestaurantId());
        return (usernameEquality && scoreEquality && commentEquality && restaurantIdEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetUserName()
    {
      return _username;
    }
    public int GetScore()
    {
      return _score;
    }
    public string GetComment()
    {
      return _comment;
    }
    public int GetRestaurantId()
    {
      return _restaurant_id;
    }
    public void SetUserName(string newUserName)
    {
      _username = newUserName;
    }
    public void SetScore(int newScore)
    {
      _score = newScore;
    }
    public void SetComment(string newComment)
    {
      _comment = newComment;
    }

    public static List<Review> GetAll()
    {
      List<Review> allReviews = new List<Review>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM reviews;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int reviewId = rdr.GetInt32(0);
        string reviewUserName = rdr.GetString(1);
        int reviewScore = rdr.GetInt32(2);
        string reviewComment = rdr.GetString(3);
        int restaurantId = rdr.GetInt32(4);
        Review newReview = new Review(reviewUserName, reviewScore, reviewComment, restaurantId, reviewId);
        allReviews.Add(newReview);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allReviews;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO reviews (username, score, comment, restaurant_id) OUTPUT INSERTED.id VALUES (@ReviewUserName, @ReviewScore, @ReviewComment, @RestaurantId);", conn);

      SqlParameter scoreParameter = new SqlParameter();
      scoreParameter.ParameterName = "@ReviewScore";
      scoreParameter.Value = this.GetScore();

      SqlParameter commentParameter = new SqlParameter();
      commentParameter.ParameterName = "@ReviewComment";
      commentParameter.Value = this.GetComment();

      SqlParameter restaurantIdParameter = new SqlParameter();
      restaurantIdParameter.ParameterName = "@RestaurantId";
      restaurantIdParameter.Value = this.GetRestaurantId();

      SqlParameter userNameParameter = new SqlParameter();
      userNameParameter.ParameterName = "@ReviewUserName";
      userNameParameter.Value = this.GetUserName();

      cmd.Parameters.Add(scoreParameter);
      cmd.Parameters.Add(commentParameter);
      cmd.Parameters.Add(restaurantIdParameter);
      cmd.Parameters.Add(userNameParameter);
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

    public static Review Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM reviews WHERE id = @ReviewId", conn);
      SqlParameter reviewIdParameter = new SqlParameter();
      reviewIdParameter.ParameterName = "@ReviewId";
      reviewIdParameter.Value = id.ToString();
      cmd.Parameters.Add(reviewIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundReviewId = 0;
      string foundReviewUserName = null;
      int foundReviewScore = 0;
      string foundReviewComment = null;
      int foundRestaurantId = 0;

      while(rdr.Read())
      {
        foundReviewId = rdr.GetInt32(0);
        foundReviewUserName = rdr.GetString(1);
        foundReviewScore = rdr.GetInt32(2);
        foundReviewComment = rdr.GetString(3);
        foundRestaurantId = rdr.GetInt32(4);
      }

      Review foundReview = new Review(foundReviewUserName, foundReviewScore, foundReviewComment, foundRestaurantId, foundReviewId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return foundReview;
    }

    public static int GetRestaurantAverage(int restaurantId)
    {
      int foundReviewAvg = 0;
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT Avg(score) FROM reviews WHERE restaurant_Id = @RestaurantId", conn);
      SqlParameter restaurantIdParameter = new SqlParameter();
      restaurantIdParameter.ParameterName = "@RestaurantId";
      restaurantIdParameter.Value = restaurantId.ToString();
      cmd.Parameters.Add(restaurantIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        foundReviewAvg = rdr.GetInt32(0);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return foundReviewAvg;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM reviews;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
