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
    }
  }
}
