using RPI.Core;
using RPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSDI.Data
{
  public static class DiagnosticsData
  {
    public static List<SensorIndication> GetSensorsLastValue(int[] sensorIds)
    {
      List<SensorIndication> lst = new List<SensorIndication>();

      using (ApplicationDbContext db = new ApplicationDbContext())
      {
        var ds = (from p in db.SensorIndications.DefaultIfEmpty()
                  where sensorIds.Contains(p.SensorId)
                  group p by p.SensorId into grp
                  let MaxDate= grp.Max(g => g.Date)
                  from p in grp
                  where p.Date == MaxDate

                  select new
                  {
                    SensorIndications = p,
                    p.Sensor,
                    p.Sensor.Device
                  }).AsEnumerable().Select(ca => ca.SensorIndications);
       

        return ds.ToList();
      }
    }

    public static List<SensorDiagnosticLog> GetSensorDiagnosticLogs(int sensorDiagnosticId, int position, int pageSize, out int totalRecords)
    {
      using (ApplicationDbContext db = new ApplicationDbContext())
      {
        var sensorDiagnosticLogs = db.SensorDiagnosticLogs.Where(x => x.SensorDiagnosticId == sensorDiagnosticId);
        totalRecords = sensorDiagnosticLogs.Count();

        return sensorDiagnosticLogs.OrderByDescending(x => x.Date).Skip(position).Take(pageSize).ToList();
      }
    }

    public static int AddDiagnosticLog(SensorDiagnosticLog sensorDiagnosticLog)
    {
      int result = 0;

      using (ApplicationDbContext db = new ApplicationDbContext())
      {
        db.SensorDiagnosticLogs.Add(sensorDiagnosticLog);
        db.SaveChanges();

        result = sensorDiagnosticLog.SensorDiagnosticLogId;
      }

      return result;
    }

    public static List<SensorDiagnostic> GetSensorDiagnostics(int sensorId, int position, int pageSize, out int totalRecords)
    {
      using (ApplicationDbContext db = new ApplicationDbContext())
      {
        var sensorDiagnostics = db.SensorDiagnostics.Include("Sensor").Where(x => x.SensorId == sensorId);
        totalRecords = sensorDiagnostics.Count();

        return sensorDiagnostics.OrderByDescending(x => x.Date).Skip(position).Take(pageSize).ToList();
      }
    }

    public static int AddDiagnostic(SensorDiagnostic sensorDiagnostic)
    {
      int result = 0;

      using (ApplicationDbContext db = new ApplicationDbContext())
      {
        db.SensorDiagnostics.Add(sensorDiagnostic);
        db.SaveChanges();

        result = sensorDiagnostic.SensorDiagnosticId;
      }

      return result;
    }
  }
}