
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using System.Web.Mvc;

namespace LupenM.WebSite.Controllers
{
  public class ErrorController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult HttpError404(string error)
    {
      ViewBag.Title = "Sorry, the page you have requested is not found.";
      ViewBag.Description = error;
      ViewBag.Type = "404";

      return View("Error");
    }

    public ActionResult HttpError500(string error)
    {
      ViewBag.Title = "Sorry, an error occurred while processing your request.";
      ViewBag.Description = error;
      ViewBag.Type = "500";

      return View("Error");
    }
  }
}