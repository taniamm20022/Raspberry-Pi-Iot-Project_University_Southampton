using CSDI.Data.Repositories.Interfaces;
using RPI.Core;
using RPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CSDI.Data
{
    public class UnitsRepository :
          GenericRepository<ApplicationDbContext, Unit>
    {

        public UnitsRepository(ApplicationDbContext context) : base(context)
        {

        }

        public static List<Unit> GetAll(int position, int pageSize, out int totalRecords)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var units = db.Units;

                totalRecords = units.Count();

                return units.OrderBy(x => x.UnitId).Skip(position).Take(pageSize).ToList();
            }
        }

        public static List<Unit> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Units.ToList();
            }
        }

       
    }
}