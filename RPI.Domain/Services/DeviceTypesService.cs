using CSDI.Data.Repositories;
using CSDI.Data.Services.Interfaces;
using RPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSDI.Data.Services
{
    public class DeviceTypesService :
        DeviceTypesRepository,
        IDeviceTypesService
    {
        #region Properties
        protected ApplicationDbContext applicationDbContext { get; }
        #endregion


        public DeviceTypesService(
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

