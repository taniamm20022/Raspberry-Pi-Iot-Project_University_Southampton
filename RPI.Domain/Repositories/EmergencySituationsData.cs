using CSDI.Data.Models;
using RPI.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CSDI.Data
{
  public class EmergencySituationsData
  {
    public static List<EmergencySituationsItem> GetAllEmergencySituations(string userId, DateTime? dateFrom, DateTime? dateTo, int? deviceId, 
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

                     select new EmergencySituationsItem()
                     {
                       Date = si.Date,
                       DeviceName = d.Name,
                       SensorName = s.Name,
                       MinValue = s.MinValue,
                       MaxValue = s.MaxValue,
                       Value = si.Value,
                       UnitName = s.Unit.Name
                     }).ToList();

        items = items.Where(i => ConvertDecimal(i.Value)!=0 && ((ConvertDecimal(i.Value) >= ConvertDecimal(i.MaxValue)) ||
                                 (ConvertDecimal(i.Value) <= ConvertDecimal(i.MinValue)))).ToList();

        totalRecords = items.Count();

        return items.OrderByDescending(x => x.Date).Skip(position).Take(pageSize).ToList();
      }
    }

    public static decimal? ConvertDecimal(string value)
    {
            decimal outValue = 0;

            if (value != null)
            {
                value = value.Replace(',', '.');

                if (!decimal.TryParse(value, NumberStyles.Any, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out outValue))
                {
                    return (Nullable<decimal>)null;
                }
            }

      return outValue;
    }
  }
}