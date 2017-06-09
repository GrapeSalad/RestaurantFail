using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using Restaurants;

namespace Restaurants.Objects
{
  public class Admin
  {
    private static string _adminName = "admin";
    private static string _adminPassword = "chimmychongs";
    private string _userNameEntry;
    private string _userPasswordEntry;
    private static bool _status;

    public Admin(string userNameEntry, string userPasswordEntry)
    {
      _userNameEntry = userNameEntry;
      _userPasswordEntry = userPasswordEntry;
    }
    public static bool GetStatus()
    {
      return _status;
    }
    public static void SetStatus(bool update)
    {
      _status = update;
    }
    public string GetNameEntry()
    {
      return _userNameEntry;
    }
    public string GetPasswordEntry()
    {
      return _userPasswordEntry;
    }
    public bool CheckPassword()
    {
      if (_userPasswordEntry == _adminPassword)
      {
        return true;
      }
      else
      {
        return false;
      }
    }
  }
}
