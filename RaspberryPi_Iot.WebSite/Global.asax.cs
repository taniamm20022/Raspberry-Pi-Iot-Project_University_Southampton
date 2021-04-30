using LupenM.WebSite.Controllers;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LupenM.WebSite
{
  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
    }

    //protected void Application_Error(object sender, EventArgs e)
    //{
    //  Exception exception = Server.GetLastError();
    //  Response.Clear();
    //  HttpException httpException = exception as HttpException;

    //  RouteData routeData = new RouteData();
    //  routeData.Values.Add("controller", "Error");

    //  if (httpException == null)
    //  {
    //    routeData.Values.Add("action", "HttpError500");
    //  }
    //  else
    //  {
    //    switch (httpException.GetHttpCode())
    //    {
    //      case 404:
    //        // Page not found.
    //        routeData.Values.Add("action", "HttpError404");
    //        break;

    //      case 500:
    //        // Server error.
    //        routeData.Values.Add("action", "HttpError500");
    //        break;

    //      default:
    //        routeData.Values.Add("action", "General");
    //        break;
    //    }
    //  }

    //  //Pass exception details to the target error View.
    //  routeData.Values.Add("error", exception);

    //  Server.ClearError();

    //  IController errorController = new ErrorController();
    //  errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
    //}
  }
}