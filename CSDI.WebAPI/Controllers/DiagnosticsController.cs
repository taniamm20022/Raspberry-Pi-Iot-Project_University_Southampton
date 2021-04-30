using CSDI.Data;
using CSDI.Data.Models;
using CSDI.WebAPI.Models;
using Microsoft.AspNet.Identity;
using RPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;


namespace LupenM.WebAPI.Controllers
{
  public class DiagnosticsController : ApiController
  {
    public ListingPageModel<DiagnosticItem> Get(int pageSize, int position)
    {
      ListingPageModel<DiagnosticItem> result = new ListingPageModel<DiagnosticItem>();
      List<DiagnosticItem> lstDiagnostics = new List<DiagnosticItem>();

      IEnumerable<Sensor> sensors;

      ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
      bool isAdmin = principal.IsInRole("admins");

      int totalRecords;
      var userId = User.Identity.GetUserId();

      if (isAdmin)
      {
        sensors = SensorsData.GetAll(position, pageSize, out totalRecords);
      }
      else
      {
        sensors = SensorsData.GetAllByPerson(userId, position, pageSize, out totalRecords);
      }

      var sensorIndications = DiagnosticsData.GetSensorsLastValue(sensors.Select(x => x.SensorId).ToArray());

      foreach (var sensor in sensors)
      {
        lstDiagnostics.Add(new DiagnosticItem
        {
          SensorId = sensor.SensorId,
          LastValue = sensorIndications.Where(X => X.SensorId == sensor.SensorId).FirstOrDefault() != null ? sensorIndications.Where(X => X.SensorId == sensor.SensorId).FirstOrDefault().Value : null,
          Date = sensorIndications.Where(X => X.SensorId == sensor.SensorId).FirstOrDefault() != null ? sensorIndications.Where(X => X.SensorId == sensor.SensorId).FirstOrDefault().Date : (DateTime?)null,
          SensorName = sensor.Name,
          Interval = sensor.IndicationInterval,
          UnitName = sensor.Unit.Name
        });
      }

      result.ListItems = lstDiagnostics;
      result.TotalRecords = totalRecords;

      return result;
    }

    public IHttpActionResult GetSensorStatus(int sensorId)
    {
      var sensorIndications = DiagnosticsData.GetSensorsLastValue(new int[] { sensorId });
      var sensorIndication = sensorIndications.Where(x => x.SensorId == sensorId).SingleOrDefault();

      if (sensorIndication != null)
      {
        return Ok(new DiagnosticItem
        {
          SensorId = sensorIndication.SensorId,
          LastValue = sensorIndication.Value,
          Date = sensorIndication.Date
        });
      }
      else return NotFound();
    }

    public ListingPageModel<DiagnosticItem> GetSensorDiagnostics(int sensorId, int pageSize, int position)
    {
      ListingPageModel<DiagnosticItem> result = new ListingPageModel<DiagnosticItem>();
      List<DiagnosticItem> lstDiagnostics = new List<DiagnosticItem>();

      int totalRecords;
      var sensorDiagnostics = DiagnosticsData.GetSensorDiagnostics(sensorId, position, pageSize, out totalRecords);

      foreach (var sensorDiagnostic in sensorDiagnostics)
      {
        lstDiagnostics.Add(new DiagnosticItem
        {
          SensorDiagnosticId = sensorDiagnostic.SensorDiagnosticId,
          SensorId = sensorDiagnostic.SensorId,
          SensorName = sensorDiagnostic.Sensor.Name,
          Date = sensorDiagnostic.Date
        });
      }

      result.ListItems = lstDiagnostics;
      result.TotalRecords = totalRecords;

      return result;
    }

    public int Post(SensorDiagnosticItem item)
    {
      int result = DiagnosticsData.AddDiagnostic(new SensorDiagnostic
      {
        SensorId = item.SensorId,
        Date = item.Date
      });

      return result;
    }
  }
}