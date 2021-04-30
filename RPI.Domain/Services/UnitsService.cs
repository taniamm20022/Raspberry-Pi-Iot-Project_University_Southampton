using CSDI.Data.Services.Interfaces;
using RPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSDI.Data.Services
{
    class UnitsService : UnitsRepository, IUnitsService
    {
        #region Properties
        protected ApplicationDbContext applicationDbContext { get; }
        #endregion


        public UnitsService(
            ApplicationDbContext context) : base(context)
        {
            applicationDbContext = context;
        }

        public void Save()
        {
            applicationDbContext.SaveChanges();
        }

    }
}
