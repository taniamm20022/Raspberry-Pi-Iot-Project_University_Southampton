using CSDI.WebAPIClient;
using System.Web.Mvc;

namespace LupenM.WebSite.Controllers
{
  public class SharedController : Controller
  {
    public ActionResult GetMainMenu()
    {
      TokenContainer container = new TokenContainer();

      return PartialView("MainMenu", container);
    }

    public ActionResult GetUserName()
    {
      TokenContainer container = new TokenContainer();

      return PartialView("_UserName", container);
    }
  }
}