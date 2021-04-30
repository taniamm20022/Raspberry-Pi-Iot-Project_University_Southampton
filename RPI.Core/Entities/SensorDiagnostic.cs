using RPI.Core.Entities;
using System;


namespace RPI.Core.Entities
{
    public class SensorDiagnostic
    {
        public int SensorDiagnosticId { get; set; }

        public int SensorId { get; set; }

        public DateTime Date { get; set; }

        public bool Status { get; set; }

        public Sensor Sensor { get; set; }
    }
}
