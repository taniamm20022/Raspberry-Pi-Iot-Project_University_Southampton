using CSDI.Data;
using CSDI.Data.Models;
using CSDI.WebAPI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


namespace LupenM.WebAPI.Controllers
{
  public class IndicationsController : ApiController
  {
    public ListingPageModel<IndicationsItem> Get(DateTime? dateFrom, DateTime? dateTo, int deviceId, int position, int pageSize)
    {
      ListingPageModel<IndicationsItem> result = new ListingPageModel<IndicationsItem>();

      ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
      bool isAdmin = principal.IsInRole("admins");
      var userId = isAdmin ? null : User.Identity.GetUserId();

      if (dateFrom.HasValue)
      {
        dateFrom = new DateTime(dateFrom.Value.Year, dateFrom.Value.Month, dateFrom.Value.Day, 0, 0, 1);
      }

      if (dateTo.HasValue)
      {
        dateTo = new DateTime(dateTo.Value.Year, dateTo.Value.Month, dateTo.Value.Day, 23, 59, 59);
      }

      int totalRecords;

      var indications = IndicationsData.GetAllIndications(userId, dateFrom, dateTo, deviceId, position, pageSize, out totalRecords);

      result.ListItems = indications;
      result.TotalRecords = totalRecords;

      return result;
    }
  }
}