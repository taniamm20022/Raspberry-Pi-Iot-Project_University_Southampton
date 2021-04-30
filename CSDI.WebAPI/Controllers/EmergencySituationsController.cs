using CSDI.Data;
using CSDI.Data.Models;
using CSDI.WebAPI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;


namespace LupenM.WebAPI.Controllers
{
  public class EmergencySituationsController : ApiController
  {
    public ListingPageModel<EmergencySituationsItem> Get(DateTime? dateFrom, DateTime? dateTo, int deviceId, int position, int pageSize)
    {
      ListingPageModel<EmergencySituationsItem> result = new ListingPageModel<EmergencySituationsItem>();

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

      var emergencySituations = EmergencySituationsData.GetAllEmergencySituations(userId, dateFrom, dateTo, deviceId, position, pageSize, out totalRecords);

      result.ListItems = emergencySituations;
      result.TotalRecords = totalRecords;

      return result;
    }
  }
}