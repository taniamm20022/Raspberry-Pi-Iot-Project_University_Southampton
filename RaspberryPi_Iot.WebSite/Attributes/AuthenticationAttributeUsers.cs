
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/



using CSDI.WebAPIClient;
using LupenM.WebSite.Controllers;
using System.Web.Mvc;




namespace LupenM.WebSite.Attributes
{
  public class AuthenticationAttributeUsers : ActionFilterAttribute
  {
    private readonly TokenContainer tokenContainer;

    public AuthenticationAttributeUsers()
    {
      tokenContainer = new TokenContainer();
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      if (tokenContainer.ApiToken == null)
      {
        var controller = (AcountsController)filterContext.Controller;
        filterContext.Result = controller.RedirectToAction("Login", "Acounts");
      }
    }
  }
}