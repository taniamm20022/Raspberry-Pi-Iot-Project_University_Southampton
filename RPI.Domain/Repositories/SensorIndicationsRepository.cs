using CSDI.Data.Models;
using RPI.Core;
using RPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CSDI.Data
{
  public static class SensorIndicationsRepository
    {
    public static List<IndicationsItem> GetAllIndications(string userId, DateTime? dateFrom, DateTime? dateTo, int? deviceId,
      int position, int pageSize, out int totalRecords)
    {
      using (ApplicationDbContext db = new ApplicationDbContext())
      {
        var items = (from si in db.SensorIndications
                     join s in db.Sensors on si.SensorId equals s.SensorId
                     join d in db.Devices on s.DeviceId equals d.DeviceId

                     where (dateFrom == null || si.Date >= dateFrom) &&
                           (dateTo == null || si.Date <= dateTo) &&
                           (deviceId == 0 || s.DeviceId == deviceId) &&
                           (userId == null || d.User.Id == userId)

                     select new IndicationsItem()
                     {
                       Date = si.Date,
                       DeviceName = d.Name,
                       IP = d.IP,
                       SensorName = s.Name,
                       Value = si.Value,
                       UnitName = s.Unit.Name
                     }).ToList();

        totalRecords = items.Count();

        return items.OrderByDescending(x => x.Date).Skip(position).Take(pageSize).ToList();
      }
    }

    public static int Add(SensorIndication indication)
    {
      using (ApplicationDbContext db = new ApplicationDbContext())
      {
        int result = 0;

        try
        {
          db.SensorIndications.Add(indication);
          db.SaveChanges();

          result = indication.SensorId;
        }
        catch (Exception ex)
        {
        }

        return result;
      }
    }
  }
}