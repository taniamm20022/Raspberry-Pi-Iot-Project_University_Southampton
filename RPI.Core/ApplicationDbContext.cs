using Microsoft.AspNet.Identity.EntityFramework;
using RPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;


namespace RPI.Core
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext() : base("ApplicationDbContext")
        {
            Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        public DbSet<IOTDevice> Devices { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<SensorIndication> SensorIndications { get; set; }
        public DbSet<SensorDiagnostic> SensorDiagnostics { get; set; }
        public DbSet<SensorDiagnosticLog> SensorDiagnosticLogs { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
