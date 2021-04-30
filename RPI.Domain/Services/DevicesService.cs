using CSDI.Data.Repositories;
using CSDI.Data.Repositories.Interfaces;
using CSDI.Data.Services;
using CSDI.Data.Services.Interfaces;
using RPI.Core;
using RPI.Core.Entities;
using System;
using System.Collections.Generic;


namespace CSDI.Data.Services
{
    public class DevicesService :
       DevicesRepository, IDevicesService
    {
        #region Properties
        protected ApplicationDbContext applicationDbContext { get; }
        #endregion


        public DevicesService(
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

