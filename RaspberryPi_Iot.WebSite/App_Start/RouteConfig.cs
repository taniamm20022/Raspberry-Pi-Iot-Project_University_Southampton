
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using System.Web.Mvc;
using System.Web.Routing;

namespace LupenM.WebSite
{
  public class RouteConfig
  {
    public const string LoginRouteName = "LogIn";

    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRoute(LoginRouteName, "log-in", new { controller = "Acounts", Action = "LogIn" });

      routes.MapRoute(
          name: "Default",
          url: "{controller}/{action}/{id}",
          defaults: new { controller = "Diagnostics", action = "Index", id = UrlParameter.Optional }
      );
    }
  }
}