using CSDI.Data.Repositories.Interfaces;
using RPI.Core;
using RPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CSDI.Data.Repositories
{
    public class DevicesRepository : GenericRepository<ApplicationDbContext,IOTDevice>
    {

        public DevicesRepository(ApplicationDbContext context): base(context)
        { 
        
        }

        public static IEnumerable<IOTDevice> GetAllDevicesForUser(string Id, int position, int pageSize, out int totalRecords)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var userDevice = db.Devices.Include("ApplicationUser").Include("DeviceType").Where(x => x.User.Id == Id);

                totalRecords = userDevice.Count();

                return userDevice.OrderBy(x => x.Date).Skip(position).Take(pageSize).ToList();
            }
        }

        public static User GetCurrentUser(string userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())

                return db.Users.FirstOrDefault(u => u.Id == userId);
        }

        public static IEnumerable<IOTDevice> GetAllDevicesForUser(string userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Devices.Include("ApplicationUser").Include("DeviceType").Where(x => x.User.Id == userId).ToList();
            }
        }

        public static IEnumerable<IOTDevice> GetAll(
            string userIdFilter, int position, int pageSize, out int totalRecords)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var ds = db.Devices.Include("ApplicationUser").Include("DeviceType").Where(a => string.IsNullOrEmpty(userIdFilter) || a.User.Id == userIdFilter);

                totalRecords = ds.Count();

                return ds.OrderBy(x => x.Date).Skip(position).Take(pageSize).ToList();
            }
        }

        public static IEnumerable<IOTDevice> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Devices.Include("ApplicationUser").Include("DeviceType").ToList();
            }
        }

    }
}
