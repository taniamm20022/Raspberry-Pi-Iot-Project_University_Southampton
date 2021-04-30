using CSDI.Data.Repositories.Interfaces;
using RPI.Core;
using RPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CSDI.Data
{
    public class SensorsRepository :
          GenericRepository<ApplicationDbContext, Sensor>
    {

        public SensorsRepository(ApplicationDbContext context) : base(context)
        {

        }

        public static List<Sensor> GetAllByPerson(string Id, int position, int pageSize, out int totalRecords)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var sensors = db.Sensors.Include("Device").Include("Unit").Where(x => x.Device.User.Id == Id);
                totalRecords = sensors.Count();

                return sensors.OrderBy(x => x.Date).Skip(position).Take(pageSize).ToList();
            }
        }

        public static List<Sensor> GetAllByPerson(string Id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Sensors.Include("Device").Include("Unit").Where(x => x.Device.User.Id == Id).ToList();
            }
        }


        public static List<Sensor> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Sensors.Include("Device").Include("Unit").ToList();
            }
        }

        public static IEnumerable<Sensor> GetAll(int position, int pageSize, out int totalRecords)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var sensors = db.Sensors.Include("Device").Include("Unit");
                totalRecords = sensors.Count();

                return sensors.OrderBy(x => x.Date).Skip(position).Take(pageSize).ToList();
            }
        }


        public static int Update(int sensorID, string name, string maxValue, string minValue, string topic,
          TimeSpan indicationInterval, int selectdUnitId, int selectedDeviceId, bool active, string location)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                int result = 0;

                try
                {
                    var entityToUpdate = db.Sensors.Where(x => x.SensorId == sensorID).SingleOrDefault();

                    entityToUpdate.Active = active;
                    entityToUpdate.Name = name;
                    entityToUpdate.MaxValue = maxValue;
                    entityToUpdate.MinValue = minValue;
                    entityToUpdate.Topic = topic;
                    entityToUpdate.IndicationInterval = indicationInterval;
                    entityToUpdate.UnitId = selectdUnitId;
                    entityToUpdate.DeviceId = selectedDeviceId;
                    entityToUpdate.Location = location;

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                }

                return result;
            }
        }

    }
}