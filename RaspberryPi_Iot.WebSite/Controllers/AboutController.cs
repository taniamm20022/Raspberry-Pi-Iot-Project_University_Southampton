using LupenM.WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LupenM.WebSite.Controllers
{
  public class AboutController : Controller
  {
    // GET: About
    public ActionResult Index()
    {
      var model = new AboutModel();
      return View(model);
    }
  }
}